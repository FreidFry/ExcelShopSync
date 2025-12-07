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
        ILogger logger,
        IWindowProvider windowProvider)
        : IWindowFactory<SettingWindow>
    {
        public SettingWindow Create()
        {
            var model = new SettingViewModel(
                serviceProvider,
                localizationManager,
                appSettings,
                logger, windowProvider);
            SettingWindow window = new(model, windowProvider);
            model.SetStorageProvider(window.StorageProvider);

            return window;
        }
    }
}
