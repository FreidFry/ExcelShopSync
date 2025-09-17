using Avalonia.Markup.Xaml;

namespace ExcelShSy.Localization;

public class LocExtension : MarkupExtension
    {
        public string Key { get; set; }

        public LocExtension(string key) => Key = key;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new Avalonia.Data.Binding($"[{Key}]")
            {
                Source = Loc.Instance
            };
        }
    }