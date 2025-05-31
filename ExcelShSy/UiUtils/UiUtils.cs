using ExcelShSy.Core.Factorys;
using ExcelShSy.Core.Interfaces.Operations;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExcelShSy.UiUtils
{
    public static class UiUtils
    {
        public static void GetExecuteTask(this List<IExecuteOperation> tasksToRun, DependencyObject parent, MyTaskFactory taskFactory)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is CheckBox cb && cb.IsChecked == true && cb.Tag is string taskName)
                {
                    try
                    {
                        var task = taskFactory.CreateTask(taskName);
                        if (task != null)
                            tasksToRun.Add(task);
                    }
                    catch (Exception)
                    {
                    }
                }

                tasksToRun.GetExecuteTask(child, taskFactory);
            }
        }
    }
}
