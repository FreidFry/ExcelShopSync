using ExcelShSy.Core.Interfaces;
using ExcelShSy.Ui.Interfaces;

namespace ExcelShSy.Ui.Factories
{
    public class SettingWindowFactory : ISettingWindowFactory
    {
        private readonly ILocalizationManager _localizationManager;
        private readonly IServiceProvider _serviceProvider;

        public SettingWindowFactory(IServiceProvider serviceProvider, ILocalizationManager localizationManager)
        {
            _serviceProvider = serviceProvider;
            _localizationManager = localizationManager;
        }

        public SettingWindow Create()
        {
            return new(_serviceProvider, _localizationManager);
        }
    }
}
