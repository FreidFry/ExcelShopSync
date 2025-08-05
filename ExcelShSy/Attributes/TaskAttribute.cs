namespace ExcelShSy.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TaskAttribute : Attribute
    {
        public string Name { get; }

        public TaskAttribute(string name) => Name = name;
    }
}
