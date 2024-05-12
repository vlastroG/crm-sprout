using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

using Consulting.Desktop.Commands;
using Consulting.Desktop.Services;
using Consulting.Models;
using Consulting.Models.Exceptions;

namespace Consulting.Desktop.ViewModels {
    public class ConsultingTaskEditingViewModel : ValidatableViewModel, ILoadEntity<ConsultingTask> {
        private ConsultingTaskStatus? _consultingTaskStatus;
        private readonly AccountService _accountService;
        private readonly IRepository<ConsultingTask> _repository;
        private readonly IRepository<ConsultingTaskStatus> _statusesRepo;
        private readonly MessageBoxService _messageBoxService;
        private ConsultingTask? _consultingTask;

        public ConsultingTaskEditingViewModel(
            AccountService accountService,
            IRepository<ConsultingTask> repository,
            IRepository<ConsultingTaskStatus> statusesRepo,
            MessageBoxService messageBoxService) {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _statusesRepo = statusesRepo ?? throw new ArgumentNullException(nameof(statusesRepo));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));

            ConsultingTaskStatuses = new ObservableCollection<ConsultingTaskStatus>();

            LoadStatusesCommand = new RelayCommandAsync(LoadTaskStatuses);
            SaveCommand = new RelayCommandAsync(Save, CanSave);
        }


        public ICommand SaveCommand { get; }

        public ICommand LoadStatusesCommand { get; }


        public int Id => _consultingTask?.Id ?? 0;

        public DateTime CreationDate => _consultingTask?.CreationDate ?? DateTime.Now;

        public string CreatorName => _consultingTask?.CreatorName ?? string.Empty;

        public string CreatorEmail => _consultingTask?.CreatorName ?? string.Empty;

        public string Description => _consultingTask?.CreatorName ?? string.Empty;

        public string CompanyService => _consultingTask?.CompanyService!.Name ?? string.Empty;

        public ObservableCollection<ConsultingTaskStatus> ConsultingTaskStatuses { get; }



        [Required]
        public ConsultingTaskStatus? ConsultingTaskStatus {
            get => _consultingTaskStatus;
            set => Set(ref _consultingTaskStatus, value);
        }

        public override string this[string columnName] {
            get {
                var error = string.Empty;
                switch(columnName) {
                    case nameof(ConsultingTaskStatus): {
                        if(ConsultingTaskStatus is null) {
                            error = "Необходимо выбрать статус";
                        }
                        break;
                    }
                    default:
                        break;
                }
                return error;
            }
        }

        public void LoadEntity(ConsultingTask consultingTask) {
            _consultingTask = consultingTask ?? throw new ArgumentNullException(nameof(consultingTask));
        }

        /// <summary>
        /// Must be invoked after <see cref="LoadEntity(ConsultingTask)"/>
        /// </summary>
        /// <returns></returns>
        private async Task LoadTaskStatuses() {
            ConsultingTaskStatuses.Clear();
            IEnumerable<ConsultingTaskStatus> statuses = await _statusesRepo.Get();
            foreach(var status in statuses) {
                ConsultingTaskStatuses.Add(status);
            }
            ConsultingTaskStatus = ConsultingTaskStatuses.First(status => status.Equals(_consultingTask?.Status));
        }

        private async Task Save() {
            _consultingTask!.Status = ConsultingTaskStatus;
            try {
                var success = await _repository.Update(_consultingTask);
                if(success) {
                    _messageBoxService.ShowInfo("Заявка успешно обновлена", "Информация");
                } else {
                    _messageBoxService.ShowError("Не удалось обновить заявку с заданными параметрами.", "Ошибка");
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
        private bool CanSave() => _consultingTask is not null && string.IsNullOrWhiteSpace(Error);
    }
}
