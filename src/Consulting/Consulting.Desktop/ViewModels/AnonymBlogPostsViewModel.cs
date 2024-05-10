using Consulting.Desktop.Services;
using Consulting.Desktop.Views;
using Consulting.Models;

namespace Consulting.Desktop.ViewModels {
    public class AnonymBlogPostsViewModel : EntityCollectionAnonymViewModel<BlogPost, BlogPostDetailsWindow> {
        public AnonymBlogPostsViewModel(
            IRepository<BlogPost> repository,
            IServiceProvider serviceProvider,
            MessageBoxService messageBoxService)
            : base(repository, serviceProvider, messageBoxService) {
        }
    }
}
