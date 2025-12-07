using Avalonia;

namespace ExcelShSy.Core.Interfaces.Operations
{
    /// <summary>
    /// Provides methods to create and execute synchronization tasks based on user selection.
    /// </summary>
    public interface IOperationTaskFactory
    {
        /// <summary>
        /// Creates an operation handler for the supplied task name.
        /// </summary>
        /// <param name="taskName">The name of the task to instantiate.</param>
        /// <returns>The operation handler instance or <c>null</c> when the task is unknown.</returns>
        IExecuteOperation? CreateTask(string taskName);

        /// <summary>
        /// Executes all operations that are active within the provided visual container.
        /// </summary>
        /// <param name="parent">The UI container that holds the task selection controls.</param>
        Task ExecuteOperations(Visual parent);
        Task ExecuteOperations(List<string> tasks);

        /// <summary>
        /// Checks whether the user has enabled at least one operation within the provided container.
        /// </summary>
        /// <param name="parent">The UI container that holds the task selection controls.</param>
        /// <returns><c>true</c> if any operation is selected; otherwise, <c>false</c>.</returns>
        bool HasCheckedCheckbox(Visual parent);
        bool HasCheckedCheckbox(int count);
    }
}
