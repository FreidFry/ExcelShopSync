using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Ui.Interfaces;
using WPFAboutF4Labs;

namespace ExcelShSy.Ui.Factories;

public class F4LabsAboutWindowFactory(ILocalizationService localizationService) : IF4LabsAboutWindowFactory
{
    public F4LabsAboutWindow Create() => new(localizationService);
}