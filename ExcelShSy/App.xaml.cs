using ExcelShSy.Core.AppConfigs;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ExcelShSy
{
    public partial class App : Application
    {
#pragma warning disable CS8618
        public App()
#pragma warning restore CS8618
        {
            Startup += OnStartup;
        }

        private ServiceProvider _provider;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();
            services.AddDependencyInjection();


            _provider = services.BuildServiceProvider();

            var main = _provider.GetRequiredService<MainWindow>();
            main.Show();
        }
    }
}
