using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows.Markup;

namespace ExcelShSy.Ui.Localization
{
    [MarkupExtensionReturnType(typeof(string))]
    public class LocExtension : MarkupExtension
    {
        private readonly string _key;
        private readonly string _resourceFile;

        public LocExtension(string fullKey)
        {
            //template: "MainWindow.Setting"
            var parts = fullKey.Split('.');
            if (parts.Length != 2)
                throw new ArgumentException("Use format: ResourceFile.Key");

            _resourceFile = parts[0];
            _key = parts[1];
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            string baseName = $"ExcelShSy.Ui.Resources.{_resourceFile}";
            var rm = new ResourceManager(baseName, Assembly.GetExecutingAssembly());
            var value = rm.GetString(_key, CultureInfo.CurrentUICulture);
            if (string.IsNullOrEmpty(value))
                return rm.GetString(_key, CultureInfo.InvariantCulture)
                ?? $"[{_key}]";
            return value;
        }
    }
}
