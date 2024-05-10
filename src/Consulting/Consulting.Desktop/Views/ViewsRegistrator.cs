using Microsoft.Extensions.DependencyInjection;

namespace Consulting.Desktop.Views {
    internal static class ViewsRegistrator {
        internal static IServiceCollection AddViews(this IServiceCollection services) => services
            .AddSingleton<MainWindow>()

            .AddTransient<AnonymMainView>()
            .AddTransient<AnonymContactsView>()
            .AddTransient<AnonymBlogPostsView>()
            .AddTransient<AnonymCompanyServicesView>()
            .AddTransient<AnonymConsultingProjectsView>()

            .AddTransient<AdminMainView>()
            .AddTransient<AdminBlogPostsView>()
            .AddTransient<AdminCompanyServicesView>()
            .AddTransient<AdminConsultingProjectsView>()
            .AddTransient<AdminConsultingTasksView>()

            .AddTransient<BlogPostCreationWindow>()
            .AddTransient<BlogPostEditingWindow>()
            .AddTransient<BlogPostDetailsWindow>()

            .AddTransient<CompanyServiceCreationWindow>()
            .AddTransient<CompanyServiceEditingWindow>()
            .AddTransient<CompanyServiceDetailsWindow>()

            .AddTransient<ConsultingProjectCreationWindow>()
            .AddTransient<ConsultingProjectEditingWindow>()
            .AddTransient<ConsultingProjectDetailsWindow>()

            .AddTransient<ConsultingTaskDetailsWindow>()
            .AddTransient<ConsultingTaskEditingWindow>()
            ;
    }
}
