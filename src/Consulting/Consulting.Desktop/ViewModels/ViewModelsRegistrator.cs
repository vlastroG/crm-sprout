using Microsoft.Extensions.DependencyInjection;

namespace Consulting.Desktop.ViewModels {
    internal static class ViewModelsRegistrator {
        internal static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddSingleton<MainWindowViewModel>()
            ;
    }
}
