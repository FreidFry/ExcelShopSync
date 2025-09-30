using System.Text.Json;
using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Settings.Properties;

public class AppSettings : IAppSettings
{
    
    private static readonly string CONFIG_FILE_NAME = $"settings.json";
    private static readonly string CONFIG_FILE = Path.Combine(Environment.CurrentDirectory, CONFIG_FILE_NAME);
    
    public string Language { get; set; } = "";
    
    private string _dataBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "F4Labs");
    public string DataBasePath
    {
        get => _dataBasePath;
        set
        {
            if (_dataBasePath != value)
            {
                _dataBasePath = value;
                SettingsChanged?.Invoke();
            }
        }
    }
    
    public bool CreateNewFileWhileSave { get; set; } = true;
    public event Action? SettingsChanged;

    public void SaveSettings(IAppSettings settings)
    {
        var json = JsonSerializer.Serialize(settings);
        File.WriteAllText(CONFIG_FILE, json);
        SettingsChanged?.Invoke();
    }

}