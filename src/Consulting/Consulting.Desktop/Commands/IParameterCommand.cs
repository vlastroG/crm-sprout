using System.Windows.Input;

namespace Consulting.Desktop.Commands {
    public interface IParameterCommand<T> : ICommand {
        void Execute(T parameter);

        bool CanExecute(T parameter);
    }
}
