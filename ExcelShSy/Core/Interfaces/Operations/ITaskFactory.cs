using System.Windows;
using System.Windows.Controls;

namespace ExcelShSy.Core.Interfaces.Operations
{
    public interface ITaskFactory
    {
        IExecuteOperation? CreateTask(string taskName);
        void RelizeExecute(DependencyObject parent);
        bool Validate(DependencyObject parent);
    }
}
