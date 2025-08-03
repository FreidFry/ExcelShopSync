using ExcelShSy.Core.Interfaces;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using ExcelShSy.Ui.Properties;

namespace ExcelShSy.Ui.Localization
{
    public class LocalizationManager : ILocalizationManager
    {
        public void SetCulture(string code)
        {
            CultureInfo? culture;
            if (string.IsNullOrEmpty(code))
                culture = new CultureInfo(CultureInfo.InstalledUICulture.Name);
            else
                culture = new CultureInfo(code);

            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            Settings.Default.Language = code;
            Settings.Default.Save();
        }

        public void SetCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            FrameworkElement.LanguageProperty.OverrideMetadata(
        typeof(FrameworkElement),
        new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(culture.IetfLanguageTag)));
        }
    }
}
