using System.ComponentModel;
using ExcelShSy.Localization.Attributes;

namespace ExcelShSy.Localization.Resources
{
    public class Enums
    {
        public enum SupportedLanguagues
        {
            Automatic,
            [Description("Українська (Ukrainian)")]
            [LangCode("uk-UA")]
            Ukrainian,
            [Description("English")]
            [LangCode("en-US")]
            English,
            [Description("Русский (Russian)")]
            [LangCode("ru-RU")]
            Russian
        }
    }
}
