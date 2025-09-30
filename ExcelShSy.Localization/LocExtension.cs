using Avalonia;
using Avalonia.Markup.Xaml;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace ExcelShSy.Localization;

public class LocExtension : MarkupExtension
    {
        private string Key { get; }

        public LocExtension()
        { }

        public LocExtension(string key) => Key = key;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget
                {
                    TargetObject: AvaloniaObject ao, TargetProperty: AvaloniaProperty ap
                }) return Loc.Instance[Key];

            Loc.Instance.PropertyChanged += (_, __) => Update();
            return Loc.Instance[Key];
            
            void Update() => ao.SetValue(ap, Loc.Instance[Key]);
        }
    }