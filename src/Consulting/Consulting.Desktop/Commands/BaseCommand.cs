using System.Windows.Input;

namespace Consulting.Desktop.Commands {
    public abstract class BaseCommand : ICommand {
        protected BaseCommand() { }


        public event EventHandler? CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }


        public abstract bool CanExecute(object? parameter);

        public abstract void Execute(object? parameter);
    }
}
