using Avalonia;
using Avalonia.Markup.Xaml;

namespace ExcelShSy.Localization;

public class LocExtension(string key) : MarkupExtension
{
        private string Key { get; } = key;
        
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget
                {
                    TargetObject: AvaloniaObject ao, TargetProperty: AvaloniaProperty ap
                }) return Loc.Instance[Key];

            Loc.Instance.PropertyChanged += (_, _) => Update();
            return Loc.Instance[Key];
            
            void Update() => ao.SetValue(ap, Loc.Instance[Key]);
        }
    }