using System.ComponentModel;
using ExcelShSy.Ui.Attributes;

namespace ExcelShSy.Ui.Resources
{
    internal class Enums
    {
        internal enum SupportedLanguagues
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
