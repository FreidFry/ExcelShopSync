using System.Globalization;
using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Core.Interfaces
{
    public interface ILocalizationManager
    {
        void SetCulture(string code);
        void SetCulture(CultureInfo culture);
    }
}
