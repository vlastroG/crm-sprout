using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using Consulting.Desktop.Commands;
using Consulting.Desktop.Services;
using Consulting.Models;
using Consulting.Models.Exceptions;

using Microsoft.Extensions.DependencyInjection;

namespace Consulting.Desktop.ViewModels {
    public abstract class EntityCollectionAnonymViewModel<TEntity, TDetailsWindow>
        : UpdatableViewModel where TEntity : Entity where TDetailsWindow : Window {
        private protected readonly IRepository<TEntity> _repository;
        private protected readonly IServiceProvider _serviceProvider;
        private protected readonly MessageBoxService _messageBoxService;

        protected EntityCollectionAnonymViewModel(
            IRepository<TEntity> repository,
            IServiceProvider serviceProvider,
            MessageBoxService messageBoxService) {

            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            Entities = new ObservableCollection<TEntity>();

            UpdateCommand = new RelayCommandAsync(Update);
            ShowDetailsCommand = new RelayCommand<TEntity>(ShowDetails, CanShowDetail);
        }


        public ObservableCollection<TEntity> Entities { get; }

        public override ICommand UpdateCommand { get; }

        public ICommand ShowDetailsCommand { get; }


        private async Task Update() {
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


        private void ShowDetails(TEntity entity) {
            var window = _serviceProvider.GetRequiredService<TDetailsWindow>();
            window.DataContext = entity;
            window.ShowDialog();
        }

        private bool CanShowDetail(TEntity entity) => entity is not null;
    }
}
