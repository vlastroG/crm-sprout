using System.Windows;

using Consulting.Desktop.Services;
using Consulting.Desktop.ViewModels;
using Consulting.Desktop.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Consulting.Desktop {
    public partial class App : System.Windows.Application {
        private readonly IHost _host;


        public App() {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services.AddViewModels();
                    services.AddViews();
                    services.AddServices();
                })
                .Build();
        }


        private async void Application_Startup(object sender, StartupEventArgs e) {
            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private async void Application_Exit(object sender, ExitEventArgs e) {
            using(_host) {
                await _host.StopAsync();
            }
        }
    }

}
