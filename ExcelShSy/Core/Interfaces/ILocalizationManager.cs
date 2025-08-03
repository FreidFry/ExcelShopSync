using System.Globalization;

namespace ExcelShSy.Core.Interfaces
{
    public interface ILocalizationManager
    {
        void SetCulture(string code);
        void SetCulture(CultureInfo culture);
    }
}
