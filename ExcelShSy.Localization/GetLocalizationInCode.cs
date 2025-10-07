using System.Globalization;
using System.Reflection;
using System.Resources;

namespace ExcelShSy.Localization
{
    public class GetLocalizationInCode
    {
        public string GetLocalizate(string resourceFile, string key)
        {
            string baseName = $"ExcelShSy.Localization.Resources.{resourceFile}";
            var rm = new ResourceManager(baseName, Assembly.GetExecutingAssembly());
            return rm.GetString(key, CultureInfo.CurrentUICulture) ?? rm.GetString(key, CultureInfo.InvariantCulture) ?? $"[{key}]";
        }
    }
}
