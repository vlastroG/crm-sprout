using System.Windows.Input;

namespace Consulting.Desktop.ViewModels {
    public class AdminConsultingTasksViewModel : UpdatableViewModel {
        public AdminConsultingTasksViewModel() {

        }

        public override ICommand UpdateCommand { get; }
    }
}
