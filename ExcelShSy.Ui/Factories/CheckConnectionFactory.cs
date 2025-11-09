using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Ui.Interfaces;
using ExcelShSy.Ui.Windows;

namespace ExcelShSy.Ui.Factories;

public class CheckConnectionFactory(ILocalizationService localizationService, ILogger logger) : ICheckConnectionFactory
{
    public CheckConnectionWindow Create() => new(localizationService, logger);
    
}