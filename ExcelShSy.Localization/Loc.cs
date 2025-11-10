using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace ExcelShSy.Localization
{
    public class Loc : INotifyPropertyChanged
    {
        private readonly string? _assemblyName;
        private readonly Dictionary<string, ResourceManager> _cache = new();

        public Loc()
        {
            _assemblyName = Assembly.GetAssembly(GetType())?.GetName().Name;
        }
        
        public static Loc Instance { get; } = new();
        public event PropertyChangedEventHandler? PropertyChanged;
        

        public void Refresh() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

        public string this[string fullKey]
        {
            get
            {
                var parts = fullKey.Split('.');
                if (parts.Length != 2) return $"[{fullKey}]";

                var rm = GetManager(parts[0]);
                
                return rm.GetString(parts[1], CultureInfo.CurrentUICulture)
                       ?? rm.GetString(parts[1], CultureInfo.InvariantCulture)
                       ?? $"[{parts[1]}]";
            }
        }
        
        private ResourceManager GetManager(string baseName)
        {
            if (!_cache.TryGetValue(baseName, out var rm))
            {
                rm = new SingleFileResourceManager(
                    $"{_assemblyName}.Resources.{baseName}",
                    typeof(Loc).Assembly);
                _cache[baseName] = rm;
            }
            return rm;
        }
    }
}