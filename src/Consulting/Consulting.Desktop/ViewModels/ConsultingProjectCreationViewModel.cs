using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Windows.Input;

using Consulting.Desktop.Commands;
using Consulting.Desktop.Services;
using Consulting.Models;
using Consulting.Models.Exceptions;

namespace Consulting.Desktop.ViewModels {
    public class ConsultingProjectCreationViewModel : ValidatableViewModel {
        private readonly AccountService _accountService;
        private readonly ImageProvider _imageProvider;
        private readonly IRepository<ConsultingProject> _repository;
        private readonly MessageBoxService _messageBoxService;
        private const int _imgMaxSize = 128 * 1024;


        public ConsultingProjectCreationViewModel(
            AccountService accountService,
            ImageProvider imageProvider,
            IRepository<ConsultingProject> repository,
            MessageBoxService messageBoxService) {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _imageProvider = imageProvider ?? throw new ArgumentNullException(nameof(imageProvider));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));

            SelectImageCommand = new RelayCommand(SelectImage);
            CreateCommand = new RelayCommandAsync(Create, CanCreate);
        }


        public ICommand SelectImageCommand { get; }

        public ICommand CreateCommand { get; }


        private string _name = string.Empty;
        [Required]
        [MaxLength(64)]
        public string Name {
            get => _name;
            set {
                if(Set(ref _name, value)) {
                    OnPropertyChanged(nameof(Error));
                }
            }
        }

        private string _description = string.Empty;
        [Required]
        [MaxLength(1024)]
        public string Description {
            get => _description;
            set {
                if(Set(ref _description, value)) {
                    OnPropertyChanged(nameof(Error));
                }
            }
        }

        private FileInfo? _image;
        public FileInfo? Image {
            get => _image;
            set {
                if(Set(ref _image, value)) {
                    OnPropertyChanged(nameof(Error));
                }
            }
        }


        public override string this[string columnName] {
            get {
                var error = string.Empty;
                switch(columnName) {
                    case nameof(Name):
                    case nameof(Description): {
                        PropertyInfo property = GetProperty(columnName);
                        error = GetStringPropertyError(property);
                        break;
                    }
                    case nameof(Image): {
                        PropertyInfo property = GetProperty(columnName);
                        error = GetFileInfoPropertyError(property, _imgMaxSize);
                        break;
                    }
                    default:
                        break;
                }
                return error;
            }
        }


        private void SelectImage() {
            try {
                Image = _imageProvider.GetImage();
            } catch(OperationCanceledException) {
                //pass
            }
        }

        private async Task Create() {
            var consultingProject = new ConsultingProject() {
                Name = Name,
                Description = Description
            };
            if(Image is not null) {
                var bytes = await File.ReadAllBytesAsync(Image.FullName);
                if(bytes is not null && bytes.Length > 0) {
                    consultingProject.Photo = bytes;
                }
            }
            try {
                var success = await _repository.Create(consultingProject);
                if(success) {
                    _messageBoxService.ShowInfo("Проект успешно добавлен, можете закрыть окно", "Информация");
                } else {
                    _messageBoxService.ShowError("Не удалось создать проект с заданными параметрами." +
                        "\nПроверьте что изображение не более 128 КБ и размером 400х225 px", "Ошибка");
                }
            } catch(ServerNotResponseException) {
                _messageBoxService.ShowError("Сервер не отвечает", "Ошибка");
            } catch(UnauthorizedUserException) {
                _messageBoxService.ShowError("Ваша сессия истекла, войдите заново", "Ошибка авторизации");
                _accountService.Logout();
            } catch(AccessDeniedException) {
                _messageBoxService.ShowError("У вас нет прав для совершения данной операции", "Ошибка");
                _accountService.Logout();
            }
        }
        private bool CanCreate() => string.IsNullOrWhiteSpace(Error);
    }
}
