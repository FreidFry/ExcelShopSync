using ExcelShSy.Core.AppConfigs;
using ExcelShSy.UiUtils;

using Microsoft.Extensions.DependencyInjection;

using System.Windows;

namespace ExcelShSy
{
    public partial class App : Application
    {
        public App()
        {
            Startup += OnStartup;
        }

        public static ServiceProvider _provider;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();
            services.AddDependencyInjection();


            _provider = services.BuildServiceProvider();
            var loc = _provider.GetRequiredService<LocalizationBinding>();
            Current.Resources.Add("Loc", loc);

            _provider.GetRequiredService<MainWindow>().Show();
        }
    }
}
