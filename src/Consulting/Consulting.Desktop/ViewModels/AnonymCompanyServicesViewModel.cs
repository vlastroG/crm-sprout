using Consulting.Desktop.Services;
using Consulting.Desktop.Views;
using Consulting.Models;

namespace Consulting.Desktop.ViewModels {
    public class AnonymCompanyServicesViewModel
        : EntityCollectionAnonymViewModel<CompanyService, CompanyServiceDetailsWindow> {
        public AnonymCompanyServicesViewModel(
            IRepository<CompanyService> repository,
            IServiceProvider serviceProvider,
            MessageBoxService messageBoxService)
            : base(repository, serviceProvider, messageBoxService) {
        }
    }
}
