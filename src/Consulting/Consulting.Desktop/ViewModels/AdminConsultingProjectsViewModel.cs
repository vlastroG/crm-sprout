using System.Windows.Input;

using Consulting.Desktop.Commands;
using Consulting.Desktop.Services;
using Consulting.Desktop.Views;
using Consulting.Models;
using Consulting.Models.Exceptions;

namespace Consulting.Desktop.ViewModels {
    public class AdminConsultingProjectsViewModel
        : EntityCollectionAdminViewModel<ConsultingProject, ConsultingProjectDetailsWindow> {
        private readonly EntityEditor _entityEditor;

        public AdminConsultingProjectsViewModel(
            EntityEditor entityEditor,
            AccountService accountService,
            IRepository<ConsultingProject> repository,
            IServiceProvider serviceProvider,
            MessageBoxService messageBoxService)
            : base(accountService, repository, serviceProvider, messageBoxService) {

            _entityEditor = entityEditor ?? throw new ArgumentNullException(nameof(entityEditor));

            EditCommand = new RelayCommand<ConsultingProject>(Edit, CanEdit);
            CreateCommand = new RelayCommand(Create);
            DeleteCommand = new RelayCommandAsync<ConsultingProject>(Delete!, CanDelete);
        }

        public override ICommand EditCommand { get; }

        public override ICommand CreateCommand { get; }

        public override ICommand DeleteCommand { get; }


        private void Create() {
            _entityEditor.CreateEntity<ConsultingProjectCreationWindow>();
            UpdateCommand.Execute(default);
        }

        private void Edit(ConsultingProject consultingProject) {
            _entityEditor.EditEntity<ConsultingProject, ConsultingProjectEditingViewModel, ConsultingProjectEditingWindow>(consultingProject);
            UpdateCommand.Execute(default);
        }
        private bool CanEdit(ConsultingProject consultingProject) => consultingProject is not null;

        private async Task Delete(ConsultingProject consultingProject) {
            if(_messageBoxService.ConfirmWarning(
                $"Вы действительно хотите удалить проект {consultingProject.Name}?",
                "Предупреждение")) {
                try {
                    var success = await _repository.Delete(consultingProject.Id);
                    if(success) {
                        _messageBoxService.ShowInfo("Проект успешно удален", "Информация");
                    } else {
                        _messageBoxService.ShowWarning("Проект уже был удален", "Предупреждение");
                    }
                } catch(ServerNotResponseException) {
                    _messageBoxService.ShowError("Сервер не отвечает", "Ошибка");
                } catch(UnauthorizedUserException) {
                    _messageBoxService.ShowError("Ваша сессия истекла, войдите заново", "Ошибка авторизации");
                    _accountService.Logout();
                } catch(AccessDeniedException) {
                    _messageBoxService.ShowError("У вас нет прав для совершения данной операции", "Ошибка удаления");
                    _accountService.Logout();
                } finally {
                    UpdateCommand.Execute(default);
                }
            }
        }
        private bool CanDelete(ConsultingProject? consultingProject) => consultingProject is not null;
    }
}
