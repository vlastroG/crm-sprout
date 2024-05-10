using System.Windows;
using System.Windows.Input;

using Consulting.Desktop.Services;
using Consulting.Models;

namespace Consulting.Desktop.ViewModels {
    public abstract class EntityCollectionAdminViewModel<TEntity, TDetailsWindow, TEditWindow, TCreateWindow>
        : EntityCollectionAnonymViewModel<TEntity, TDetailsWindow>
        where TEntity : Entity
        where TDetailsWindow : Window
        where TEditWindow : Window
        where TCreateWindow : Window {

        protected EntityCollectionAdminViewModel(
            IRepository<TEntity> repository,
            IServiceProvider serviceProvider,
            MessageBoxService messageBoxService)
            : base(repository, serviceProvider, messageBoxService) {
        }


        public ICommand EditCommand { get; }

        public ICommand CreateCommand { get; }

        public ICommand DeleteCommand { get; }



    }
}
