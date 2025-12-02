using Avalonia.Data.Converters;
using System.ComponentModel;
using System.Globalization;

namespace ExcelShSy.Ui.Utils
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            var type = value.GetType();
            var field = type.GetField(value.ToString());
            if (field == null) return value.ToString();

            var attr = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attr?.Description ?? value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // если нужно двустороннее связывание — можно реализовать
        }
    }
}
