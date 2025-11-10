using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Ui.Interfaces;
using ExcelShSy.Ui.Utils;

namespace ExcelShSy.Ui.Factories;

public class UpdateManagerFactory(ILocalizationService localizationService, IAppSettings appSettings, ILogger logger) : IUpdateManagerFactory
{
    public UpdateManager Create() => new(localizationService, appSettings, logger);
}
