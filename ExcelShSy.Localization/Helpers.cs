using System.ComponentModel;
using System.Reflection;
using ExcelShSy.Localization.Attributes;
using ExcelShSy.Localization.Resources;

namespace ExcelShSy.Localization
{
    public static class Helpers
    {
        public static T[] GetEnums<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            if (fi == null) return value.ToString();
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static string GetLangCode(this Enums.SupportedLanguagues value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<LangCodeAttribute>();
            return attr?.Code ?? string.Empty;
        }
    }
}
