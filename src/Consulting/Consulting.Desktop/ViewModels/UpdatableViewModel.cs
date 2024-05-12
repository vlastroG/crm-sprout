using System.Windows.Input;

namespace Consulting.Desktop.ViewModels {
    public abstract class UpdatableViewModel : BaseViewModel {
        protected UpdatableViewModel() : base() {

        }

        public abstract ICommand UpdateCommand { get; }
    }
}
