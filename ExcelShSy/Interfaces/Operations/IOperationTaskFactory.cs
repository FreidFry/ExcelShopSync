using Avalonia;

namespace ExcelShSy.Core.Interfaces.Operations
{
    public interface IOperationTaskFactory
    {
        IExecuteOperation? CreateTask(string taskName);
        Task ExecuteOperations(Visual parent);
        bool HasCheckedCheckbox(Visual parent);
    }
}
