using System.Windows.Input;

using Consulting.Desktop.Commands;

namespace Consulting.Desktop.ViewModels {
    public class AnonymContactsViewModel : UpdatableViewModel {
        public AnonymContactsViewModel() {
            UpdateCommand = new RelayCommand(Update);
        }

        public override ICommand UpdateCommand { get; }

        private void Update() { }
    }
}
