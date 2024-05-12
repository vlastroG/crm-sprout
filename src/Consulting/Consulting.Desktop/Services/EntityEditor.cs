using System.Windows;

using Consulting.Desktop.ViewModels;
using Consulting.Models;

using Microsoft.Extensions.DependencyInjection;

namespace Consulting.Desktop.Services {
    public class EntityEditor {
        private readonly IServiceProvider _serviceProvider;

        public EntityEditor(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }


        public void CreateEntity<TCreationWindow>() where TCreationWindow : Window {
            var window = _serviceProvider.GetRequiredService<TCreationWindow>();
            window.ShowDialog();
        }

        public void EditEntity<TEntity, TEditingViewModel, TWindow>(TEntity entity)
            where TEntity : Entity
            where TEditingViewModel : BaseViewModel, ILoadEntity<TEntity>
            where TWindow : Window {

            var viewModel = _serviceProvider.GetRequiredService<TEditingViewModel>();
            viewModel.LoadEntity(entity);
            var window = _serviceProvider.GetRequiredService<TWindow>();
            window.DataContext = viewModel;
            window.ShowDialog();
        }
    }
}
