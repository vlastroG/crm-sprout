using Consulting.Desktop.Services;
using Consulting.Desktop.Views;
using Consulting.Models;

namespace Consulting.Desktop.ViewModels {
    public class AnonymConsultingProjectsViewModel
        : EntityCollectionAnonymViewModel<ConsultingProject, ConsultingProjectDetailsWindow> {
        public AnonymConsultingProjectsViewModel(
            IRepository<ConsultingProject> repository,
            IServiceProvider serviceProvider,
            MessageBoxService messageBoxService)
            : base(repository, serviceProvider, messageBoxService) {
        }
    }
}
