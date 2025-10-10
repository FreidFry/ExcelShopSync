using System.Text.Json;
using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Settings.Properties;

public class AppSettings : IAppSettings
{
    
    private static readonly string ConfigFileName = $"settings.json";
    private static readonly string ConfigFile = Path.Combine(Environment.CurrentDirectory, ConfigFileName);
    
    public string Language { get; set; } = "";

    public string DataBasePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "F4Labs");

    public bool CreateNewFileWhileSave { get; set; } = true;

    public bool CheckForUpdates { get; set; } = true;
    public DateTime LastUpdateCheck { get; set; } = DateTime.Now.Date;
    public event Action? SettingsChanged;

    public void SaveSettings(IAppSettings settings)
    {
        var json = JsonSerializer.Serialize(settings);
        File.WriteAllText(ConfigFile, json);
        Reload();
        SettingsChanged?.Invoke();
    }

    private void Reload()
    {
        if (!File.Exists(ConfigFile))
            return;

        var json = File.ReadAllText(ConfigFile);
        var loaded = JsonSerializer.Deserialize<AppSettings>(json);

        if (loaded is not null)
        {
            Language = loaded.Language;
            DataBasePath = loaded.DataBasePath;
            CreateNewFileWhileSave = loaded.CreateNewFileWhileSave;
        }
    }
}