using System.Windows.Input;

using Consulting.Desktop.Commands;

namespace Consulting.Desktop.ViewModels {
    public class AnonymMainViewModel : UpdatableViewModel {
        public AnonymMainViewModel(ConsultingTaskCreationViewModel taskCreationViewModel) {
            TaskCreationViewModel = taskCreationViewModel
                ?? throw new ArgumentNullException(nameof(taskCreationViewModel));

            UpdateCommand = new RelayCommand(Update);
        }

        public override ICommand UpdateCommand { get; }

        public ConsultingTaskCreationViewModel TaskCreationViewModel { get; }

        private void Update() {
            TaskCreationViewModel.LoadCompanyServicesCommand.Execute(default);
        }
    }
}
