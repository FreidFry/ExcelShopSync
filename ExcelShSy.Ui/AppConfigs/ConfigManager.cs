using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Settings.Properties;

namespace ExcelShSy.Ui.AppConfigs;

public class ConfigManager : IConfigurationManager, INotifyPropertyChanged
{
    private static readonly string CONFIG_FILE_NAME = $"settings.json";
    private static readonly string CONFIG_FILE = Path.Combine(Environment.CurrentDirectory, CONFIG_FILE_NAME);
    
    public IAppSettings Load()
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
            else
            {
                // на всякий fallback
                finalValue = loadedValue == null ? defaultValue : loadedValue;
            }

            prop.SetValue(result, finalValue);
        }

        return result;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}