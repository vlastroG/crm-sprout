using System.Windows;
using System.Windows.Input;

using Consulting.Desktop.Services;
using Consulting.Models;

namespace Consulting.Desktop.ViewModels {
    public abstract class EntityCollectionAdminViewModel<TEntity, TDetailsWindow>
        : EntityCollectionAnonymViewModel<TEntity, TDetailsWindow>
        where TEntity : Entity
        where TDetailsWindow : Window {

        private protected readonly AccountService _accountService;

        protected EntityCollectionAdminViewModel(
            AccountService accountService,
            IRepository<TEntity> repository,
            IServiceProvider serviceProvider,
            MessageBoxService messageBoxService)
            : base(repository, serviceProvider, messageBoxService) {
            _accountService = accountService;
        }


        public abstract ICommand EditCommand { get; }

        public abstract ICommand CreateCommand { get; }

        public abstract ICommand DeleteCommand { get; }
    }
}
