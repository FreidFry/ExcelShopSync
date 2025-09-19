using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Settings.Properties;

public class AppSettings : IAppSettings
{
    public string Language { get; set; } = "";
    public string DataBasePath {get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "F4Labs");
}