using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Windows.Input;

using Consulting.Desktop.Commands;
using Consulting.Desktop.Services;
using Consulting.Models;
using Consulting.Models.Exceptions;

namespace Consulting.Desktop.ViewModels {
    public class BlogPostCreationViewModel : ValidatableViewModel, IDataErrorInfo {
        private readonly AccountService _accountService;
        private readonly ImageProvider _imageProvider;
        private readonly IRepository<BlogPost> _repository;
        private readonly MessageBoxService _messageBoxService;

        public BlogPostCreationViewModel(
            AccountService accountService,
            ImageProvider imageProvider,
            IRepository<BlogPost> repository,
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
        [MaxLength(32)]
        public string Name {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _contentShort = string.Empty;
        [Required]
        [MaxLength(256)]
        public string ContentShort {
            get => _contentShort;
            set => Set(ref _contentShort, value);
        }

        private string? _contentFull;
        [MaxLength(1024)]
        public string? ContentFull {
            get => _contentFull;
            set => Set(ref _contentFull, value);
        }

        private FileInfo? _image;
        public FileInfo? Image {
            get => _image;
            set => Set(ref _image, value);
        }


        public override string this[string columnName] {
            get {
                var error = string.Empty;
                switch(columnName) {
                    case nameof(Name):
                    case nameof(ContentShort):
                    case nameof(ContentFull): {
                        PropertyInfo property = GetType().GetProperties().First(prop => prop.Name == columnName);
                        error = GetStringPropertyError(property);
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
            var blogPost = new BlogPost() {
                Name = Name,
                ContentShort = ContentShort,
                ContentFull = ContentFull
            };
            if(Image is not null) {
                var bytes = await File.ReadAllBytesAsync(Image.FullName);
                if(bytes is not null && bytes.Length > 0) {
                    blogPost.Photo = bytes;
                }
            }
            try {
                var success = await _repository.Create(blogPost);
                if(success) {
                    _messageBoxService.ShowInfo("Пост успешно добавлен, можете закрыть окно", "Информация");
                } else {
                    _messageBoxService.ShowError("Не удалось создать пост с заданными параметрами." +
                        "\nПроверьте что изображение не более 128 КБ и размером 400х225 px", "Ошибка");
                }
            } catch(ServerNotResponseException) {
                _messageBoxService.ShowError("Сервер не отвечает", "Ошибка");
            } catch(UnauthorizedUserException) {
                _messageBoxService.ShowError("Ваша сессия истекла, войдите заново", "Ошибка авторизации");
                _accountService.Logout();
            } catch(AccessDeniedException) {
                _messageBoxService.ShowError("У вас нет прав для совершения данной операции", "Ошибка удаления");
                _accountService.Logout();
            }
        }
        private bool CanCreate() => string.IsNullOrWhiteSpace(Error);
    }
}
