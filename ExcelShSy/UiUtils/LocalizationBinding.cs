using ExcelShSy.Core.Interfaces.Common;

using System.ComponentModel;
using System.Reflection;
using System.Resources;

namespace ExcelShSy.UiUtils
{
    public class LocalizationBinding : INotifyPropertyChanged
    {
        private readonly ILocalizationService _localizationService;
        private readonly ResourceManager _mainWindowRm;

        public LocalizationBinding(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
            _mainWindowRm = new ResourceManager("ExcelShSy.Resources.MainWindow", Assembly.GetExecutingAssembly());

            _localizationService.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(_localizationService.CurrentCulture))
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            };
        }

        public string this[string key] =>
    _mainWindowRm.GetString(key, Thread.CurrentThread.CurrentUICulture)
    ?? $"[{key}]";

        public event PropertyChangedEventHandler? PropertyChanged;
    }

}
