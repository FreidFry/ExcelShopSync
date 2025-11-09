using ExcelShSy.Core.Interfaces;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Ui.Interfaces;
using ExcelShSy.Ui.Windows;

namespace ExcelShSy.Ui.Factories
{
    public class SettingWindowFactory(
        IServiceProvider serviceProvider,
        ILocalizationManager localizationManager,
        IAppSettings appSettings)
        : ISettingWindowFactory
    {
        public SettingWindow Create()
        {
            return new(serviceProvider, localizationManager, appSettings);
        }
    }
}
