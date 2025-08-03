using System.ComponentModel;
using System.Reflection;
using System.Resources;

namespace ExcelShSy.Ui.Utils
{
    internal static class EnumExtensions
    {
        internal static string GetLocalizedDescription(this Enum value)
        {
            return value.GetLocalizedDescription();
        }

        public static string GetLocalizedDescription(this Enum value, ResourceManager resourceManager)
        {
            string resourceName = value.GetType().Name + "_" + value;
            string description = resourceManager.GetString(resourceName);

            if (string.IsNullOrEmpty(description))
            {
                description = value.GetDescription();
            }

            return description;
        }
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi != null)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
            }

            return value.ToString();
        }
    }
}
