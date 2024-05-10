using System.Windows.Input;

namespace Consulting.Desktop.ViewModels {
    public class AnonymCompanyServicesViewModel : UpdatableViewModel {
        public AnonymCompanyServicesViewModel() {

        }

        public override ICommand UpdateCommand { get; }
    }
}
