using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace ExcelShSy.Localization
{
    public class Loc : INotifyPropertyChanged
    {
        public static Loc Instance { get; } = new();
        public event PropertyChangedEventHandler? PropertyChanged;

        public void Refresh() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

        public string this[string fullKey]
        {
            get
            {
                var parts = fullKey.Split('.');
                if (parts.Length != 2) return $"[{fullKey}]";

                var rm = new ResourceManager($"ExcelShSy.Localization.Resources.{parts[0]}",
                    Assembly.GetExecutingAssembly());
                return rm.GetString(parts[1], CultureInfo.CurrentUICulture)
                       ?? rm.GetString(parts[1], CultureInfo.InvariantCulture)
                       ?? $"[{parts[1]}]";
            }
        }
    }
}