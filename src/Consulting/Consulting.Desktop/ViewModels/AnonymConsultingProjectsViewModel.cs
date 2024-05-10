using System.Windows.Input;

namespace Consulting.Desktop.ViewModels {
    public class AnonymConsultingProjectsViewModel : UpdatableViewModel {
        public AnonymConsultingProjectsViewModel() {

        }

        public override ICommand UpdateCommand { get; }
    }
}
