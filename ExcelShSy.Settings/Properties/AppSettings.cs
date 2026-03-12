using ExcelShSy.Core.Enums;
using ExcelShSy.Core.Helpers;
using ExcelShSy.Core.Interfaces.Common;
using System.Text.Json;

namespace ExcelShSy.Settings.Properties;

public class AppSettings : IAppSettings
{

    private static readonly string ConfigFileName = $"settings.json";
    private static readonly string ConfigFile = Path.Combine(Environment.CurrentDirectory, ConfigFileName);

    private string _languageCode = "";

    public string LanguageCode
    {
        get => _languageCode;
        set => _languageCode = value;
    }

    public LanguaguesEnum.SupportedLanguagues Language
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_languageCode))
                return LanguaguesEnum.SupportedLanguagues.Automatic;
            return EnumHelper.GetEnumValueFromAttribute(_languageCode);
        }
        set => _languageCode = value == LanguaguesEnum.SupportedLanguagues.Automatic 
            ? string.Empty : value.GetLangCode();
    }

    public string DataBasePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "F4Labs");

    public bool CreateNewFileWhileSave { get; set; } = true;

    public bool AutoCheckUpdate { get; set; } = true;
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
            AutoCheckUpdate = loaded.AutoCheckUpdate;
            LastUpdateCheck = loaded.LastUpdateCheck;
        }
    }
}