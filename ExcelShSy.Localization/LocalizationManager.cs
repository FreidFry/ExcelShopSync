using System.Globalization;
using System.Text.Json;
using Avalonia;
using ExcelShSy.Core.Interfaces;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Localization.Properties;

namespace ExcelShSy.Localization
{
    public class LocalizationManager : ILocalizationManager
    {
        private const string SettingsFile = "settings.json";
        
        public event EventHandler? LanguageChanged;
        
        public void SetCulture(string code)
        {
            CultureInfo culture;

            if (string.IsNullOrEmpty(code))
                culture = new CultureInfo(CultureInfo.InstalledUICulture.Name);
            else
                culture = new CultureInfo(code);

            ApplyCulture(culture);
        }

        public void SaveSettings(IAppSettings settings)
        {
            var json = JsonSerializer.Serialize(settings);
            File.WriteAllText(SettingsFile, json);
        }

        private void ApplyCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            SaveSettings(new AppSettings { Language = culture.Name });

            // уведомляем UI через Loc.Instance
            Loc.Instance.Refresh();
            
            // сигналим всем, кто подписан
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SetCulture(CultureInfo culture)
        {
            ApplyCulture(culture);
        }
    }
}