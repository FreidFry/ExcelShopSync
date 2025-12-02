using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Ui.Interfaces;
using WPFAboutF4Labs;

namespace ExcelShSy.Ui.Factories;

public class F4LabsAboutWindowFactory(ILocalizationService localizationService, ILogger logger) : IWindowFactory<F4LabsAboutWindow>
{
    public F4LabsAboutWindow Create() => new(localizationService, logger);
}