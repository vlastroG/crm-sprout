using System.Windows.Input;

using Consulting.Desktop.ViewModels;

namespace Consulting.Desktop.Commands {
    public class RelayCommandAsync<T> : BaseViewModel, IParameterCommand<T> {
        private readonly Func<T?, Task> _execute;
        private readonly Func<T?, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommandAsync(Func<T?, Task> execute, Func<T?, bool>? canExecute = default) {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommandAsync(Func<Task> execute, Func<bool>? canExecute = default) {
            if(execute is null) { throw new ArgumentNullException(nameof(execute)); }
            _execute = p => execute();
            _canExecute = canExecute is not null ? p => canExecute() : default;
        }


        private bool _isExecuting;
        public bool IsExecuting {
            get => _isExecuting;
            set => Set(ref _isExecuting, value);
        }

        bool ICommand.CanExecute(object? parameter) {
            return CanExecute((T?) parameter);
        }

        void ICommand.Execute(object? parameter) {
            Execute((T?) parameter);
        }


        public bool CanExecute(T? parameter) {
            return !IsExecuting && (_canExecute is null || _canExecute(parameter));
        }

        public async void Execute(T? parameter) {
            await ExecuteAsync(parameter);
        }

        public async Task ExecuteAsync(T? parameter) {
            IsExecuting = true;
            try {
                await _execute(parameter);
            } finally {
                IsExecuting = false;
            }
        }
    }
}
