using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Ui.Interfaces;
using ExcelShSy.Ui.Windows;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Ui.Factories;

public class CheckConnectionFactory(ILocalizationService localizationService, ILogger logger, IMessages<IMsBox<ButtonResult>> messages) : ICheckConnectionFactory
{
    public CheckConnectionWindow Create() => new(localizationService, logger, messages);
    
}