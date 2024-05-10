using Microsoft.Extensions.DependencyInjection;

namespace Consulting.Desktop.ViewModels {
    internal static class ViewModelsRegistrator {
        internal static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddSingleton<MainWindowViewModel>()

            .AddTransient<AnonymMainViewModel>()
            .AddTransient<AnonymCompanyServicesViewModel>()
            .AddTransient<AnonymConsultingProjectsViewModel>()
            .AddTransient<AnonymBlogPostsViewModel>()
            .AddSingleton<AnonymContactsViewModel>()

            .AddTransient<AdminConsultingTasksViewModel>()
            .AddTransient<AdminConsultingProjectsViewModel>()
            .AddTransient<AdminCompanyServicesViewModel>()
            .AddTransient<AdminBlogPostsViewModel>()

            .AddTransient<ConsultingTaskCreationViewModel>()
            .AddTransient<ConsultingTaskEditingViewModel>()

            .AddTransient<ConsultingProjectCreationViewModel>()
            .AddTransient<ConsultingProjectEditingViewModel>()

            .AddTransient<CompanyServiceCreationViewModel>()
            .AddTransient<CompanyServiceEditingViewModel>()

            .AddTransient<BlogPostCreationViewModel>()
            .AddTransient<BlogPostEditingViewModel>()
            ;
    }
}
