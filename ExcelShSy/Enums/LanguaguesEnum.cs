using System.ComponentModel;
using ExcelShSy.Core.Attributes;

namespace ExcelShSy.Core.Enums
{
    public class LanguaguesEnum
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
