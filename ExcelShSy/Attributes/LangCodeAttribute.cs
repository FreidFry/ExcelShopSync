namespace ExcelShSy.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LangCodeAttribute : Attribute
    {
        public string Code { get; }

        public LangCodeAttribute(string code)
        {
            Code = code;
        }
    }
}
