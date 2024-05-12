using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Input;

using Consulting.Desktop.Commands;
using Consulting.Desktop.Services;
using Consulting.Models;
using Consulting.Models.Exceptions;

namespace Consulting.Desktop.ViewModels {
    public class CompanyServiceEditingViewModel : ValidatableViewModel, ILoadEntity<CompanyService> {
        private readonly AccountService _accountService;
        private readonly IRepository<CompanyService> _repository;
        private readonly MessageBoxService _messageBoxService;
        private CompanyService? _companyService;


        public CompanyServiceEditingViewModel(
            AccountService accountService,
            IRepository<CompanyService> repository,
            MessageBoxService messageBoxService) {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));

            SaveCommand = new RelayCommandAsync(Save, CanSave);
        }


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


        public void LoadEntity(CompanyService companyService) {
            _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
            Name = _companyService.Name;
            Description = _companyService.Description;
        }

        private async Task Save() {
            _companyService!.Name = Name;
            _companyService.Description = Description;
            try {
                var success = await _repository.Update(_companyService);
                if(success) {
                    _messageBoxService.ShowInfo("Услуга успешно сохранена, можете закрыть окно", "Информация");
                } else {
                    _messageBoxService.ShowError("Не удалось сохранить услугу с заданными параметрами.", "Ошибка");
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
        private bool CanSave() => _companyService is not null && string.IsNullOrWhiteSpace(Error);
    }
}
