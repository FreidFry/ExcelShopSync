using System.ComponentModel;
using System.Globalization;

namespace ExcelShSy.Core.Interfaces.Common
{
    public interface ILocalizationService : INotifyPropertyChanged
    {
        CultureInfo CurrentCulture { get; }
        void SetCulture(string cultureName);
    }
}