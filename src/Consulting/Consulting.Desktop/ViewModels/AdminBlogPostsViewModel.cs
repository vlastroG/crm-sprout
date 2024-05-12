using System.Windows.Input;

using Consulting.Desktop.Commands;
using Consulting.Desktop.Services;
using Consulting.Desktop.Views;
using Consulting.Models;
using Consulting.Models.Exceptions;

namespace Consulting.Desktop.ViewModels {
    public class AdminBlogPostsViewModel : EntityCollectionAdminViewModel<BlogPost, BlogPostDetailsWindow> {
        private readonly EntityEditor _entityEditor;

        public AdminBlogPostsViewModel(
            AccountService accountService,
            IRepository<BlogPost> repository,
            IServiceProvider serviceProvider,
            MessageBoxService messageBoxService,
            EntityEditor entityEditor)
            : base(accountService, repository, serviceProvider, messageBoxService) {

            _entityEditor = entityEditor ?? throw new ArgumentNullException(nameof(entityEditor));

            EditCommand = new RelayCommand<BlogPost>(Edit, CanEdit);
            CreateCommand = new RelayCommand(Create);
            DeleteCommand = new RelayCommandAsync<BlogPost>(Delete!, CanDelete);
        }


        public override ICommand EditCommand { get; }

        public override ICommand CreateCommand { get; }

        public override ICommand DeleteCommand { get; }


        private void Create() {
            _entityEditor.CreateEntity<BlogPostCreationWindow>();
            UpdateCommand.Execute(default);
        }

        private void Edit(BlogPost blogPost) {
            _entityEditor.EditEntity<BlogPost, BlogPostEditingViewModel, BlogPostEditingWindow>(blogPost);
            UpdateCommand.Execute(default);
        }
        private bool CanEdit(BlogPost blogPost) => blogPost is not null;

        private async Task Delete(BlogPost blogPost) {
            if(_messageBoxService.ConfirmWarning(
                $"Вы действительно хотите удалить пост {blogPost.Name}?",
                "Предупреждение")) {
                try {
                    var success = await _repository.Delete(blogPost.Id);
                    if(success) {
                        _messageBoxService.ShowInfo("Пост успешно удален", "Информация");
                    } else {
                        _messageBoxService.ShowWarning("Пост уже был удален", "Предупреждение");
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
        private bool CanDelete(BlogPost? blogPost) => blogPost is not null;
    }
}
