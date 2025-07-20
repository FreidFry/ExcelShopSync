using System.Windows;

namespace ExcelShSy.Core.Interfaces.Operations
{
    public interface ITaskFactory
    {
        IExecuteOperation? CreateTask(string taskName);
        void RelizeExecute(DependencyObject parent);
        bool Validate(DependencyObject parent);
    }
}
