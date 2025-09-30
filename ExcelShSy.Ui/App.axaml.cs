using ExcelShSy.Core.Interfaces;
using ExcelShSy.Ui.AppConfigs;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Ui
{
    public class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly ILocalizationManager _localizationManager;
        private readonly IAppSettings _appSettings;
        public App()
        {
            var services = new ServiceCollection();
            services.AddDependencyInjection();
            
            _appSettings = ConfigManager.Load();
            services.AddSingleton(_appSettings);
            
            _serviceProvider = services.BuildServiceProvider();
            _localizationManager = _serviceProvider.GetRequiredService<ILocalizationManager>();
        }
        
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }


        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var culture = new CultureInfo(_appSettings.Language);
                _localizationManager.SetCulture(culture);

                // Создание главного окна
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}
