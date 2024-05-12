using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Windows.Input;

using Consulting.Desktop.Commands;
using Consulting.Desktop.Services;
using Consulting.Models;
using Consulting.Models.Exceptions;

namespace Consulting.Desktop.ViewModels {
    public class BlogPostEditingViewModel : ValidatableViewModel, ILoadEntity<BlogPost> {
        private readonly AccountService _accountService;
        private readonly ImageProvider _imageProvider;
        private readonly IRepository<BlogPost> _repository;
        private readonly MessageBoxService _messageBoxService;
        private const int _imgMaxSize = 128 * 1024;
        private BlogPost? _blogPost;

        public BlogPostEditingViewModel(
            AccountService accountService,
            ImageProvider imageProvider,
            IRepository<BlogPost> repository,
            MessageBoxService messageBoxService) {

            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _imageProvider = imageProvider ?? throw new ArgumentNullException(nameof(imageProvider));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));

            SelectImageCommand = new RelayCommand(SelectImage);
            SaveCommand = new RelayCommandAsync(Save, CanSave);
        }


        public ICommand SelectImageCommand { get; }

        public ICommand SaveCommand { get; }


        private string _name = string.Empty;
        [Required]
        [MaxLength(32)]
        public string Name {
            get => _name;
            set {
                if(Set(ref _name, value)) {
                    OnPropertyChanged(nameof(Error));
                }
            }
        }

        private string _contentShort = string.Empty;
        [Required]
        [MaxLength(256)]
        public string ContentShort {
            get => _contentShort;
            set {
                if(Set(ref _contentShort, value)) {
                    OnPropertyChanged(nameof(Error));
                }
            }
        }

        private string? _contentFull;
        [MaxLength(1024)]
        public string? ContentFull {
            get => _contentFull;
            set {
                if(Set(ref _contentFull, value)) {
                    OnPropertyChanged(nameof(Error));
                }
            }
        }

        private FileInfo? _newImage;
        public FileInfo? NewImage {
            get => _newImage;
            set {
                if(Set(ref _newImage, value)) {
                    OnPropertyChanged(nameof(Error));
                }
            }
        }

        private byte[]? _currentphoto;
        public byte[]? CurrentPhoto {
            get => _currentphoto;
            set => Set(ref _currentphoto, value);
        }



        public override string this[string columnName] {
            get {
                var error = string.Empty;
                switch(columnName) {
                    case nameof(Name):
                    case nameof(ContentShort):
                    case nameof(ContentFull): {
                        PropertyInfo property = GetProperty(columnName);
                        error = GetStringPropertyError(property);
                        break;
                    }
                    case nameof(NewImage): {
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

        public void LoadEntity(BlogPost blogPost) {
            _blogPost = blogPost ?? throw new ArgumentNullException(nameof(blogPost));
            Name = blogPost.Name;
            ContentShort = blogPost.ContentShort;
            ContentFull = blogPost.ContentFull;
            CurrentPhoto = blogPost.Photo;
        }



        private void SelectImage() {
            try {
                NewImage = _imageProvider.GetImage();
            } catch(OperationCanceledException) {
                //pass
            }
        }

        private async Task Save() {
            _blogPost!.Name = Name;
            _blogPost.ContentShort = ContentShort;
            _blogPost.ContentFull = ContentFull;
            if(NewImage is not null) {
                var bytes = await File.ReadAllBytesAsync(NewImage.FullName);
                if(bytes is not null && bytes.Length > 0) {
                    _blogPost.Photo = bytes;
                }
            }
            try {
                var success = await _repository.Update(_blogPost);
                if(success) {
                    _messageBoxService.ShowInfo("Пост успешно изменен, можете закрыть окно", "Информация");
                } else {
                    _messageBoxService.ShowError("Не удалось изменить пост с заданными параметрами." +
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
        private bool CanSave() => _blogPost is not null && string.IsNullOrWhiteSpace(Error);
    }
}
