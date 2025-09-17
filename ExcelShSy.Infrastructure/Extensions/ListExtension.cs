using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Infrastructure.Factories;

using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace ExcelShSy.Infrastructure.Extensions
{
    public static class ListExtension
    {
        public static bool IsNullOrEmpty([NotNullWhen(false)] this IList<string>? list)
        {
            return list == null || list.Count == 0;
        }
        public static void AddOperationTask(this List<IExecuteOperation> tasksToRun, Visual parent, OperationTaskFactory taskFactory)
        {
            foreach (var child in parent.GetVisualChildren())
            {
                if (child is CheckBox cb && cb.IsChecked == true && cb.Tag is string taskName)
                {
                    try
                    {
                        var task = taskFactory.CreateTask(taskName);
                        if (task != null)
                            tasksToRun.Add(task);
                    }
                    catch
                    {
                    }
                }

                // рекурсивно для дочерних элементов
                if (child is Visual visual)
                    tasksToRun.AddOperationTask(visual, taskFactory);
            }
        }

    }
}
