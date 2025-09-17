using Avalonia;
using Avalonia.Controls.Primitives;

namespace ExcelShSy.Core.Interfaces.Operations
{
    public interface IOperationTaskFactory
    {
        IExecuteOperation? CreateTask(string taskName);
        void ExecuteOperations(Visual parent);
        bool HasCheckedCheckbox(Visual parent);
    }
}
