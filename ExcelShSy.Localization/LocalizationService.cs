using System.Globalization;
using System.Reflection;
using System.Resources;
using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Localization;

public class LocalizationService : ILocalizationService
{
    private string AssemblyName { get; }
    
    private readonly Dictionary<string, ResourceManager> _cache = new();
    
    public LocalizationService()
    {
        AssemblyName = Assembly.GetAssembly(GetType())!.GetName().Name!;
    }

    public string GetString(string resourceFile, string key)
    {
        var rm = GetManager(resourceFile);
        return rm.GetString(key, CultureInfo.CurrentUICulture) ?? rm.GetString(key, CultureInfo.InvariantCulture) ?? $"[{key}]";
    }
    
    public string GetErrorString(string key)
    {
        return GetString("Errors", key);
    }

    public string GetMessageString(string key)
    {
        return GetString("MessagesBox", key);
    }
    
    private ResourceManager GetManager(string baseName)
    {
        if (!_cache.TryGetValue(baseName, out var rm))
        {
            rm = new ResourceManager(
                $"{AssemblyName}.Resources.{baseName}",
                typeof(Loc).Assembly);
            _cache[baseName] = rm;
        }
        return rm;
    }
}