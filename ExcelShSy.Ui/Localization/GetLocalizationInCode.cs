using System.Globalization;
using System.Reflection;
using System.Resources;

namespace ExcelShSy.Ui.Localization
{
    public static class GetLocalizationInCode
    {
        public static string GetLocalizate(string resourceFile, string key)
        {
            string baseName = $"ExcelShSy.Ui.Resources.{resourceFile}";
            var rm = new ResourceManager(baseName, Assembly.GetExecutingAssembly());
            return rm.GetString(key, CultureInfo.CurrentUICulture) ?? rm.GetString(key, CultureInfo.InvariantCulture) ?? $"[{key}]";
        }
    }
}
