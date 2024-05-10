using Consulting.Desktop.Services;
using Consulting.Models;

namespace Consulting.Desktop.ViewModels {
    public class AdminBlogPostsViewModel : AnonymBlogPostsViewModel {
        public AdminBlogPostsViewModel(
            IRepository<BlogPost> repository,
            IServiceProvider serviceProvider,
            MessageBoxService messageBoxService)
            : base(repository, serviceProvider, messageBoxService) {
        }
    }
}
