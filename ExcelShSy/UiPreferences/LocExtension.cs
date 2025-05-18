using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ExcelShSy.UiPreferences
{
    internal class LocExtension : MarkupExtension
    {
        string Key { get; }
        public LocExtension(string key) => Key = key;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var rm = new ResourceManager("ExcelShSy.Resources.Buttons", Assembly.GetExecutingAssembly());
            return rm.GetString(Key, Thread.CurrentThread.CurrentUICulture) ?? $"[{Key}]";
        }
    }
}
