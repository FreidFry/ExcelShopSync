using System.Reflection;
using System.Text.Json;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Settings.Properties;

namespace ExcelShSy.Ui.AppConfigs;

public sealed class ConfigManager : IConfigurationManager
{
    private const string ConfigFileName = "settings.json";
    private static readonly string ConfigFile = Path.Combine(Environment.CurrentDirectory, ConfigFileName);
    
    public IAppSettings Load()
    {
        var defaults =  new AppSettings();
        
        if (!File.Exists(ConfigFile))
            return defaults;
        try
        {
            var json = File.ReadAllText(ConfigFile);
            var loaded = JsonSerializer.Deserialize<AppSettings>(json);
            
            return loaded == null ? defaults : Merge(defaults, loaded);
        }
        catch
        {
            return defaults;
        }
    }

    private static AppSettings Merge(AppSettings defaults, AppSettings merged)
    {
        var result = new AppSettings();

        foreach (var prop in typeof(AppSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!prop.CanRead || !prop.CanWrite)
                continue;

            var loadedValue = prop.GetValue(merged);
            var defaultValue = prop.GetValue(defaults);

            object? finalValue;

            if (loadedValue == null)
                finalValue = defaultValue;
            else if (prop.PropertyType == typeof(string))
                finalValue = string.IsNullOrWhiteSpace((string?)loadedValue) ? defaultValue : loadedValue;
            else
                finalValue = loadedValue;

            prop.SetValue(result, finalValue);
        }

        return result;
    }
}