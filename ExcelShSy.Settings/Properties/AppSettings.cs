using System.Text.Json;
using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Settings.Properties;

public class AppSettings : IAppSettings
{
    
    private static readonly string CONFIG_FILE_NAME = $"settings.json";
    private static readonly string CONFIG_FILE = Path.Combine(Environment.CurrentDirectory, CONFIG_FILE_NAME);
    
    public string Language { get; set; } = "";
    public string DataBasePath {get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "F4Labs");
    
    public void SaveSettings(IAppSettings settings)
    {
        var json = JsonSerializer.Serialize(settings);
        File.WriteAllText(CONFIG_FILE, json);
    }

}