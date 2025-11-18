namespace ExcelShSy.Core.Attributes
{
    /// <summary>
    /// Provides metadata for identifying operation tasks by a friendly name.
    /// </summary>
    /// <param name="name">The display name of the task.</param>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TaskAttribute(string name) : Attribute
    {
        /// <summary>
        /// Gets the display name associated with the annotated task.
        /// </summary>
        public string Name { get; } = name;
    }
}
