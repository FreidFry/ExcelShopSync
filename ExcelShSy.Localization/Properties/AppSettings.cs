using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Localization.Properties;

public class AppSettings : IAppSettings
{
    public string Language { get; set; } = "en";
}