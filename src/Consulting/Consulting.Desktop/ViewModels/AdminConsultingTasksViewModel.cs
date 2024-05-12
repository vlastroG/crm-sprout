using System.Windows.Input;

using Consulting.Desktop.Commands;
using Consulting.Desktop.Services;
using Consulting.Desktop.Views;
using Consulting.Models;
using Consulting.Models.Exceptions;

namespace Consulting.Desktop.ViewModels {
    public class AdminConsultingTasksViewModel
        : EntityCollectionAdminViewModel<ConsultingTask, ConsultingTaskDetailsWindow> {
        private readonly EntityEditor _entityEditor;

        public AdminConsultingTasksViewModel(
            EntityEditor entityEditor,
            AccountService accountService,
            IRepository<ConsultingTask> repository,
            IServiceProvider serviceProvider,
            MessageBoxService messageBoxService)
            : base(accountService, repository, serviceProvider, messageBoxService) {

            _entityEditor = entityEditor ?? throw new ArgumentNullException(nameof(entityEditor));

            EditCommand = new RelayCommand<ConsultingTask>(Edit, CanEdit);
            UpdateCommand = new RelayCommandAsync(Update);
            FilterByDateCommand = new RelayCommandAsync(FilterByDate, CanFilterByDate);
            ResetFilterCommand = new RelayCommand(ResetFilter);
        }

        public override ICommand UpdateCommand { get; }

        public override ICommand EditCommand { get; }

        public override ICommand CreateCommand => throw new NotSupportedException();

        public override ICommand DeleteCommand => throw new NotSupportedException();

        public ICommand FilterByDateCommand { get; }

        public ICommand ResetFilterCommand { get; }


        private DateTime? _startDate;
        public DateTime? StartDate {
            get => _startDate;
            set => Set(ref _startDate, value);
        }


        private DateTime? _endDate;
        public DateTime? EndDate {
            get => _endDate;
            set => Set(ref _endDate, value);
        }

        private async Task Update() {
            StartDate = null;
            EndDate = null;
            Entities.Clear();
            try {
                var entities = await _repository.Get();
                foreach(var entity in entities) {
                    Entities.Add(entity);
                }
            } catch(ServerNotResponseException) {
                _messageBoxService.ShowError("Сервер не отвечает", "Ошибка загрузки окна");
            }
        }

        private async Task FilterByDate() {
            Entities.Clear();
            try {
                var entities = await _repository.Get();
                foreach(var entity in entities) {
                    if(StartDate <= entity.CreationDate && entity.CreationDate <= (EndDate + TimeSpan.FromDays(1))) {
                        Entities.Add(entity);
                    }
                }
            } catch(ServerNotResponseException) {
                _messageBoxService.ShowError("Сервер не отвечает", "Ошибка загрузки окна");
            }
        }
        private bool CanFilterByDate() => StartDate is not null && EndDate is not null && StartDate < EndDate;

        private void ResetFilter() {
            UpdateCommand.Execute(default);
        }


        private void Edit(ConsultingTask consultingTask) {
            _entityEditor.EditEntity<ConsultingTask, ConsultingTaskEditingViewModel, ConsultingTaskEditingWindow>(consultingTask);
            UpdateCommand.Execute(default);
        }
        private bool CanEdit(ConsultingTask consultingTask) => consultingTask is not null;
    }
}
