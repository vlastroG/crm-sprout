using Consulting.Models;

using Microsoft.Extensions.DependencyInjection;

namespace Consulting.Desktop.Services {
    internal static class ServicesRegistrator {
        internal static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddHttpClient()
            .AddSingleton<AccountService>()
            .AddSingleton<MessageBoxService>()
            .AddSingleton<IRepository<BlogPost>, BlogPostsRepository>()
            .AddSingleton<IRepository<CompanyService>, CompanyServicesRepository>()
            .AddSingleton<IRepository<ConsultingProject>, ConsultingProjectsRepository>()
            .AddSingleton<IRepository<ConsultingTask>, ConsultingTasksRepository>()
            .AddSingleton<IRepository<ConsultingTaskStatus>, ConsultingTaskStatusesRepository>()
            .AddSingleton<EntityEditor>()
            .AddSingleton<ImageProvider>()
            ;
    }
}
