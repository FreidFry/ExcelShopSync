namespace ExcelShSy.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TaskAttribute(string name) : Attribute
    {
        public string Name { get; } = name;
    }
}
