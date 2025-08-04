using ExcelShSy.Core.Interfaces;
using ExcelShSy.Ui.AppConfigs;
using ExcelShSy.Localization.Properties;

using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Windows;

namespace ExcelShSy.Ui
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        private ILocalizationManager _localizationManager;

        public App()
        {
            var services = new ServiceCollection();
            services.AddDependencyInjection();
            _serviceProvider = services.BuildServiceProvider();
            _localizationManager = _serviceProvider.GetRequiredService<ILocalizationManager>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string langCode = Settings.Default.Language;
            if (langCode == null || string.IsNullOrEmpty(langCode))
                langCode = CultureInfo.InstalledUICulture.Name;
            var culture = new CultureInfo(langCode);

            _localizationManager.SetCulture(culture);

            this.ShutdownMode = ShutdownMode.OnMainWindowClose;

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
