using System.Windows.Input;

namespace Consulting.Desktop.Commands {
    public abstract class BaseCommand<T> : IParameterCommand<T> {
        protected BaseCommand() { }


        public event EventHandler? CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        bool ICommand.CanExecute(object? parameter) {
            return CanExecute((T?) parameter);
        }

        void ICommand.Execute(object? parameter) {
            Execute((T?) parameter);
        }

        public abstract bool CanExecute(T? parameter);

        public abstract void Execute(T? parameter);
    }
}
