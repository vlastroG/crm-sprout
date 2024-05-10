namespace Consulting.Desktop.Commands {
    public class RelayCommand : BaseCommand {
        private readonly Action<object> _execute;
        private readonly Func<object, bool>? _canExecute;


        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null) : base() {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommand(Action execute, Func<bool>? canExecute = default) {
            if(execute is null) { throw new ArgumentNullException(nameof(execute)); }
            _execute = p => execute();
            _canExecute = canExecute is not null ? p => canExecute() : default;
        }


        public override bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter!) ?? true;

        public override void Execute(object? parameter) {
            if(!CanExecute(parameter))
                return;
            _execute(parameter!);
        }
    }
}
