using System.ComponentModel;
using System.Reflection;
using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Enums;

namespace ExcelShSy.Core.Helpers
{
    public static class EnumHelper
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

        public static string GetLangCode(this LanguaguesEnum.SupportedLanguagues value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<LangCodeAttribute>();
            return attr?.Code ?? string.Empty;
        }

        public static T GetEnumValue<T>(string value)
        {
            Enum.TryParse(typeof(T), value, out var result);
            return (T)result!;
        }

        public static T GetEnumValueFromAttribute<T, TAttribute>(string value)
            where T : Enum
            where TAttribute : LangCodeAttribute
        {
            foreach(var field in typeof(T).GetFields())
            {
                var attribute = field.GetCustomAttribute<TAttribute>();
                if (attribute != null && attribute.Code == value)
                    return (T)field.GetValue(null)!;
            }
            return default!;
        }

        public static LanguaguesEnum.SupportedLanguagues GetEnumValueFromAttribute(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                foreach (var field in typeof(LanguaguesEnum.SupportedLanguagues).GetFields())
                {
                    var attribute = field.GetCustomAttribute<LangCodeAttribute>();
                    if (attribute != null && attribute.Code == value)
                        return (LanguaguesEnum.SupportedLanguagues)field.GetValue(null)!;
                }
            return default!;
        }
    }
}
