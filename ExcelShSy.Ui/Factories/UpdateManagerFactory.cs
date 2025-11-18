using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Ui.Interfaces;
using ExcelShSy.Ui.Utils;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Ui.Factories;

public class UpdateManagerFactory(ILocalizationService localizationService, IAppSettings appSettings, ILogger logger, IMessages<IMsBox<ButtonResult>> messages) : IUpdateManagerFactory
{
    public UpdateManager Create() => new(localizationService, appSettings, logger, messages);
}
