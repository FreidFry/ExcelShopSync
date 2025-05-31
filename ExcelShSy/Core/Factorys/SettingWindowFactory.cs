using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Core.Factorys
{
    public class SettingWindowFactory : ISettingWindowFactory
    {
        private readonly ILocalizationService _localizationService;

        public SettingWindowFactory(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public SettingWindow Create()
        {
            return new(_localizationService);
        }
    }
}
