using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Infrastructure.Factories;

using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace ExcelShSy.Infrastructure.Extensions
{
    /// <summary>
    /// Provides list-related helper extensions used across the infrastructure layer.
    /// </summary>
    public static class ListExtension
    {
        /// <summary>
        /// Determines whether the list is null or contains no elements.
        /// </summary>
        /// <param name="list">The list to inspect.</param>
        /// <returns><c>true</c> if the list is null or empty; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrEmpty([NotNullWhen(false)] this IList<string>? list)
        {
            return list == null || list.Count == 0;
        }

        /// <summary>
        /// Recursively walks a visual tree to collect operation tasks from checked checkboxes.
        /// </summary>
        /// <param name="tasksToRun">The list to populate with tasks.</param>
        /// <param name="parent">The parent visual to inspect.</param>
        /// <param name="taskFactory">The factory used to instantiate tasks.</param>
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
                    { }
                }

                // рекурсивно для дочерних элементов
                if (child is Visual visual)
                    tasksToRun.AddOperationTask(visual, taskFactory);
            }
        }

        public static void AddOperationTask(this List<IExecuteOperation> tasksToRun, List<string> parent, OperationTaskFactory taskFactory)
        {
            foreach (var taskName in parent)
            {
                try
                {
                    var task = taskFactory.CreateTask(taskName);
                    if (task != null) tasksToRun.Add(task);
                }
                catch
                { }
            }
        }

    }
}
