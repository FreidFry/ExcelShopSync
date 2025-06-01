using ExcelShSy.Core.Interfaces.Common;

using System.ComponentModel;
using System.Globalization;

namespace ExcelShSy.Ui.Utils
{
    public class LocalizationService : ILocalizationService
    {
        private CultureInfo _currentCulture = Thread.CurrentThread.CurrentUICulture;

        public CultureInfo CurrentCulture
        {
            get => _currentCulture;
            private set
            {
                if (!_currentCulture.Equals(value))
                {
                    _currentCulture = value;
                    Thread.CurrentThread.CurrentCulture = value;
                    Thread.CurrentThread.CurrentUICulture = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentCulture)));
                }
            }
        }

        public void SetCulture(string cultureName)
        {
            SetCulture(new CultureInfo(cultureName));
        }

        public void SetCulture(CultureInfo culture)
        {
            CurrentCulture = culture;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

}
