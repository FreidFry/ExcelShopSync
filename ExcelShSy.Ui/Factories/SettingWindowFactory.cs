using ExcelShSy.Core.Interfaces;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Ui.Interfaces;

namespace ExcelShSy.Ui.Factories
{
    public class SettingWindowFactory : ISettingWindowFactory
    {
        private readonly ILocalizationManager _localizationManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAppSettings _appSettings;

        public SettingWindowFactory(IServiceProvider serviceProvider, ILocalizationManager localizationManager, IAppSettings  appSettings)
        {
            _serviceProvider = serviceProvider;
            _localizationManager = localizationManager;
            _appSettings = appSettings;
        }

        public SettingWindow Create()
        {
            return new(_serviceProvider, _localizationManager, _appSettings);
        }
    }
}
