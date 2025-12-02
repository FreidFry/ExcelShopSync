using ExcelShSy.Core.Interfaces;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Ui.Interfaces;
using ExcelShSy.Ui.ModelView.View;
using ExcelShSy.Ui.Windows;

namespace ExcelShSy.Ui.Factories
{
    public class SettingWindowFactory(
        IServiceProvider serviceProvider,
        ILocalizationManager localizationManager,
        IAppSettings appSettings,
        ILogger logger)
        : IWindowFactory<SettingWindow>
    {
        public SettingWindow Create()
        {
            var model = new SettingViewModel(
                serviceProvider,
                localizationManager,
                appSettings,
                logger);
            SettingWindow window = new(model);
            model.SetStorageProvider(window.StorageProvider);

            return window;
        }
    }
}
