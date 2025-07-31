using System.Windows;

namespace ExcelShSy.Core.Interfaces.Operations
{
    public interface IOperationTaskFactory
    {
        IExecuteOperation? CreateTask(string taskName);
        void ExecuteOperations(DependencyObject parent);
        bool IsAnyCheckboxChecked(DependencyObject parent);
    }
}
