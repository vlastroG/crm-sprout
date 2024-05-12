using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Input;

using Consulting.Desktop.Commands;
using Consulting.Desktop.Services;
using Consulting.Models;
using Consulting.Models.Exceptions;

namespace Consulting.Desktop.ViewModels {
    public class CompanyServiceCreationViewModel : ValidatableViewModel {
        private readonly AccountService _accountService;
        private readonly IRepository<CompanyService> _repository;
        private readonly MessageBoxService _messageBoxService;

        public CompanyServiceCreationViewModel(
            AccountService accountService,
            IRepository<CompanyService> repository,
            MessageBoxService messageBoxService) {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));

            CreateCommand = new RelayCommandAsync(Create, CanCreate);
        }


        public ICommand CreateCommand { get; }


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

        private string _description = string.Empty;
        [Required]
        [MaxLength(512)]
        public string Description {
            get => _description;
            set {
                if(Set(ref _description, value)) {
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
                    default:
                        break;
                }
                return error;
            }
        }

        private async Task Create() {
            var companyService = new CompanyService() {
                Name = Name,
                Description = Description
            };
            try {
                var success = await _repository.Create(companyService);
                if(success) {
                    _messageBoxService.ShowInfo("Услуга успешно добавлена, можете закрыть окно", "Информация");
                } else {
                    _messageBoxService.ShowError("Не удалось создать услугу с заданными параметрами." +
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
