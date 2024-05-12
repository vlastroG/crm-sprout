namespace Consulting.Desktop.Commands {
    public class RelayCommand<T> : BaseCommand<T> {
        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;


        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null) : base() {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommand(Action execute, Func<bool>? canExecute = default) {
            if(execute is null) { throw new ArgumentNullException(nameof(execute)); }
            _execute = p => execute();
            _canExecute = canExecute is not null ? p => canExecute() : default;
        }


        public override bool CanExecute(T? parameter) => _canExecute?.Invoke(parameter!) ?? true;

        public override void Execute(T? parameter) {
            if(!CanExecute(parameter))
                return;
            _execute(parameter!);
        }
    }

    public class RelayCommand : RelayCommand<object> {
        public RelayCommand(Action execute, Func<bool>? canExecute = default) : base(execute, canExecute) {

        }
    }
}
