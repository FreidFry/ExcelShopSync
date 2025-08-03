using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Infrastructure.Factories;

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExcelShSy.Infrastructure.Extensions
{
    public static class ListExtension
    {
        public static bool IsNullOrEmpty([NotNullWhen(false)] this IList<string>? list)
        {
            return list == null || list.Count == 0;
        }
        public static void AddOperationTask(this List<IExecuteOperation> tasksToRun, DependencyObject parent, OperationTaskFactory taskFactory)
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

                tasksToRun.AddOperationTask(child, taskFactory);
            }
        }
    }
}
