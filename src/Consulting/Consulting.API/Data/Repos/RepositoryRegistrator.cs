using Consulting.Models;

namespace Consulting.API.Data.Repos {
    public static class RepositoryRegistrator {
        public static IServiceCollection AddRepositories(this IServiceCollection services) => services
            .AddScoped<IRepository<BlogPost>, BlogPostsRepository>()
            .AddScoped<IRepository<CompanyService>, CompanyServicesRepository>()
            .AddScoped<IRepository<CompanyStatement>, CompanyStatementsRepository>()
            .AddScoped<IRepository<ConsultingProject>, ConsultingProjectsRepository>()
            .AddScoped<IRepository<ConsultingTask>, ConsultingTasksRepository>()
            ;
    }
}
