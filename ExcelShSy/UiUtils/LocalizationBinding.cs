using ExcelShSy.Core.Interfaces.Common;

using System.ComponentModel;
using System.Reflection;
using System.Resources;

namespace ExcelShSy.UiUtils
{
    public class LocalizationBinding : INotifyPropertyChanged
    {
        private readonly ILocalizationService _localizationService;
        private readonly ResourceManager _rm;

        public LocalizationBinding(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
            _rm = new ResourceManager("ExcelShSy.Resources.Buttons", Assembly.GetExecutingAssembly());

            _localizationService.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(_localizationService.CurrentCulture))
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            };
        }

        public string this[string key] =>
            _rm.GetString(key, Thread.CurrentThread.CurrentUICulture) ?? $"[{key}]";

        public event PropertyChangedEventHandler? PropertyChanged;
    }

}
