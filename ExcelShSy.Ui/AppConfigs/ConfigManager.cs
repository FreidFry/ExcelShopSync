using System.Reflection;
using System.Text.Json;
using ExcelShSy.Settings.Properties;

namespace ExcelShSy.Ui.AppConfigs;

public class ConfigManager
{
    private static readonly string CONFIG_FILE_NAME = $"settings.json";
    private static readonly string CONFIG_FILE = Path.Combine(Environment.CurrentDirectory, CONFIG_FILE_NAME);

    public static AppSettings Load()
    {
        var defaults =  new AppSettings();
        
        if (!File.Exists(CONFIG_FILE))
            return defaults;
        try
        {
            string json = File.ReadAllText(CONFIG_FILE);
            var loaded = JsonSerializer.Deserialize<AppSettings>(json);
            
            if (loaded == null)
                return defaults;
            
            return Merge(defaults, loaded);
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

            object? finalValue = null;

            if (loadedValue == null)
            {
                finalValue = defaultValue;
            }
            else if (prop.PropertyType == typeof(string))
            {
                finalValue = string.IsNullOrWhiteSpace((string?)loadedValue) ? defaultValue : loadedValue;
            }
            else if (prop.PropertyType.IsValueType)
            {
                // если это число/структура и равно "дефолту", то берём из defaults
                finalValue = loadedValue.Equals(Activator.CreateInstance(prop.PropertyType))
                    ? defaultValue
                    : loadedValue;
            }
            else
            {
                // на всякий fallback
                finalValue = loadedValue ?? defaultValue;
            }

            prop.SetValue(result, finalValue);
        }

        return result;
    }
}