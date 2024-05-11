using System.Windows.Input;

using Consulting.Desktop.Commands;
using Consulting.Desktop.Services;
using Consulting.Desktop.Views;
using Consulting.Models;
using Consulting.Models.Exceptions;

namespace Consulting.Desktop.ViewModels {
    public class AdminCompanyServicesViewModel
        : EntityCollectionAdminViewModel<CompanyService, CompanyServiceDetailsWindow> {
        private readonly EntityEditor _entityEditor;

        public AdminCompanyServicesViewModel(
            EntityEditor entityEditor,
            AccountService accountService,
            IRepository<CompanyService> repository,
            IServiceProvider serviceProvider,
            MessageBoxService messageBoxService)
            : base(accountService, repository, serviceProvider, messageBoxService) {

            _entityEditor = entityEditor ?? throw new ArgumentNullException(nameof(entityEditor));

            EditCommand = new RelayCommand<CompanyService>(Edit, CanEdit);
            CreateCommand = new RelayCommand(Create);
            DeleteCommand = new RelayCommandAsync<CompanyService>(Delete!, CanDelete);
        }


        public override ICommand EditCommand { get; }

        public override ICommand CreateCommand { get; }

        public override ICommand DeleteCommand { get; }


        private void Create() {
            _entityEditor.CreateEntity<CompanyServiceCreationWindow>();
            UpdateCommand.Execute(default);
        }

        private void Edit(CompanyService companyService) {
            _entityEditor.EditEntity<CompanyService, CompanyServiceEditingViewModel, CompanyServiceEditingWindow>(companyService);
            UpdateCommand.Execute(default);
        }
        private bool CanEdit(CompanyService companyService) => companyService is not null;

        private async Task Delete(CompanyService companyService) {
            if(_messageBoxService.ConfirmWarning(
                $"Вы действительно хотите удалить услугу {companyService.Name}?",
                "Предупреждение")) {
                try {
                    var success = await _repository.Delete(companyService.Id);
                    if(success) {
                        _messageBoxService.ShowInfo("Услуга успешно удалена", "Информация");
                    } else {
                        _messageBoxService.ShowWarning("Услуга уже была удалена", "Предупреждение");
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
        private bool CanDelete(CompanyService? companyService) => companyService is not null;
    }
}
