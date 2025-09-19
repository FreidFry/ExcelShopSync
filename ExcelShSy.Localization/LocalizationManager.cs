using System.Globalization;
using ExcelShSy.Core.Interfaces;

namespace ExcelShSy.Localization
{
    public class LocalizationManager : ILocalizationManager
    {
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
        
        private void ApplyCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            
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