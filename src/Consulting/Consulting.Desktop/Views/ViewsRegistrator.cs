using Microsoft.Extensions.DependencyInjection;

namespace Consulting.Desktop.Views {
    internal static class ViewsRegistrator {
        internal static IServiceCollection AddViews(this IServiceCollection services) => services
            .AddSingleton<MainWindow>()
            ;
    }
}
