using System.Windows.Controls;

namespace ExcelShSy.Core.Interfaces.Operations
{
    public interface ITaskFactory
    {
        IExecuteOperation CreateTask(string taskName);
        void RelizeExecute(Grid TaskGrid);
        bool Validate(Grid TaskGrid);
    }
}
