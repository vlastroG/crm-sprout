using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Input;

using Consulting.Desktop.Commands;
using Consulting.Desktop.Services;
using Consulting.Models;
using Consulting.Models.Exceptions;

namespace Consulting.Desktop.ViewModels {
    public class ConsultingTaskCreationViewModel : ValidatableViewModel {
        private readonly AccountService _accountService;
        private readonly IRepository<ConsultingTask> _repository;
        private readonly IRepository<CompanyService> _servicesRepo;
        private readonly MessageBoxService _messageBoxService;

        public ConsultingTaskCreationViewModel(
            AccountService accountService,
            IRepository<ConsultingTask> repository,
            IRepository<CompanyService> servicesRepo,
            MessageBoxService messageBoxService) {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _servicesRepo = servicesRepo ?? throw new ArgumentNullException(nameof(servicesRepo));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));

            CompanyServices = new ObservableCollection<CompanyService>();

            CreateCommand = new RelayCommandAsync(Create, CanCreate);
            LoadCompanyServicesCommand = new RelayCommandAsync(LoadCompanyServices);
        }

        public ICommand CreateCommand { get; }
        public ICommand LoadCompanyServicesCommand { get; }

        public ObservableCollection<CompanyService> CompanyServices { get; }

        private string _creatorName = string.Empty;
        [Required]
        [MaxLength(64)]
        public string CreatorName {
            get => _creatorName;
            set {
                if(Set(ref _creatorName, value)) {
                    OnPropertyChanged(nameof(Error));
                };
            }
        }

        private string _creatorEmail = string.Empty;
        [Required]
        [EmailAddress]
        public string CreatorEmail {
            get => _creatorEmail;
            set {
                if(Set(ref _creatorEmail, value)) {
                    OnPropertyChanged(nameof(Error));
                };
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

        private CompanyService? _companyService;
        [Required]
        public CompanyService? CompanyService {
            get => _companyService;
            set => Set(ref _companyService, value);
        }

        public override string this[string columnName] {
            get {
                var error = string.Empty;
                switch(columnName) {
                    case nameof(CreatorName):
                    case nameof(CreatorEmail):
                    case nameof(Description): {
                        PropertyInfo property = GetProperty(columnName);
                        error = GetStringPropertyError(property);
                        break;
                    }
                    case nameof(CompanyService): {
                        if(CompanyService is null) {
                            error = "Необходимо выбрать услугу";
                        }
                        break;
                    }
                    default:
                        break;
                }
                return error;
            }
        }


        private async Task LoadCompanyServices() {
            CompanyServices.Clear();
            IEnumerable<CompanyService> services = await _servicesRepo.Get();
            foreach(var service in services) {
                CompanyServices.Add(service);
            }
            CompanyService = CompanyServices.FirstOrDefault();
        }

        private async Task Create() {
            var consultingTask = new ConsultingTask() {
                Status = new ConsultingTaskStatus(),
                CreatorName = CreatorName,
                CreatorEmail = CreatorEmail,
                Description = Description,
                CompanyService = CompanyService
            };
            try {
                var success = await _repository.Create(consultingTask);
                if(success) {
                    _messageBoxService.ShowInfo("Заявка успешно создана", "Информация");
                } else {
                    _messageBoxService.ShowError("Не удалось создать заявку с заданными параметрами.", "Ошибка");
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
