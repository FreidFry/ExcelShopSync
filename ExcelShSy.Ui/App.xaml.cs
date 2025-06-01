using ExcelShSy.Ui.AppConfigs;
using ExcelShSy.Ui.Utils;

using Microsoft.Extensions.DependencyInjection;

using System.Windows;

namespace ExcelShSy.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            services.AddDependencyInjection();
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loc = _serviceProvider.GetRequiredService<LocalizationBinding>();
            Current.Resources.Add("Loc", loc);

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _serviceProvider.Dispose();
        }
    }
}
