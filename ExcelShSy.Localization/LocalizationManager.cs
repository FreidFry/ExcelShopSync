using System.Globalization;
using ExcelShSy.Core.Interfaces;

namespace ExcelShSy.Localization
{
    public class LocalizationManager : ILocalizationManager
    {
        public void SetCulture(string code)
        {
            CultureInfo culture = string.IsNullOrEmpty(code) ? new (CultureInfo.InstalledUICulture.Name) : new (code);
            ApplyCulture(culture);
        }
        
        public void SetCulture(CultureInfo culture)
        {
            ApplyCulture(culture);
        }
        
        private void ApplyCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            
            Loc.Instance.Refresh();
        }
    }
}