using ExcelShSy.Core.Factorys;
using ExcelShSy.Core.Interfaces.Operations;

using System.Windows.Controls;

namespace ExcelShSy.UiUtils
{
    public static class UiUtils
    {
        public static void GetExecuteTask(this List<IExecuteOperation> tasksToRun, Grid TaskGrid, MyTaskFactory taskFactory)
        {
            foreach (var child in TaskGrid.Children)
            {
                if (child is CheckBox cb && cb.IsChecked == true)
                {
                    string? taskName = cb.Tag?.ToString();
                    if (!string.IsNullOrEmpty(taskName))

                        try
                        {
                            var task = taskFactory.CreateTask(taskName);
                            tasksToRun.Add(task);
                        }
                        catch (Exception ex)
                        {
                        }
                }
            }
        }
    }
}
