using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Infrastructure.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using Avalonia;
using MsBox.Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace ExcelShSy.Infrastructure.Factories
{
    public class OperationTaskFactory : IOperationTaskFactory
    {
        readonly IServiceProvider _serviceProvider;
        readonly ILogger _logger;
        private readonly Dictionary<string, Type> _tasksMap;

        public OperationTaskFactory(IServiceProvider services, ILogger logger)
        {
            _serviceProvider = services;
            _logger = logger;

            var allTasks = _serviceProvider.GetServices<IExecuteOperation>();

            _tasksMap = allTasks
                .Select(t => t.GetType())
                .Where(t => t.GetCustomAttribute<TaskAttribute>() != null)
                .ToDictionary(
                    t => t.GetCustomAttribute<TaskAttribute>().Name,
                    t => t);
        }

        public IExecuteOperation? CreateTask(string taskName)
        {
            try
            {
                _tasksMap.TryGetValue(taskName, out var taskType);
                return (IExecuteOperation?)_serviceProvider.GetRequiredService(taskType);
            }
            catch
            {
                return null;
            }
        }

        public void ExecuteOperations(Visual parent)
        {
            HashSet<string> errors = [];

            List<IExecuteOperation> tasksToRun = [];
            tasksToRun.AddOperationTask(parent, this);
            _logger.LogInfo("Executes: " + string.Join(", ", tasksToRun.Select(t => t.GetType().Name)));
            var saveTask = CreateTask("SavePackages");
            if (saveTask != null)
                tasksToRun.Add(saveTask);
            foreach (var task in tasksToRun)
            {
                _logger.LogInfo($"Start {task.GetType().Name}");
                task.Execute();
                if (task.Errors.Count != 0)
                    foreach (var error in task.Errors)
                        errors.Add(error);

                _logger.LogInfo($"Finish {task.GetType().Name}");
            }

            if (errors.Count == 0)
            {
                _logger.LogInfo("Finish without errors.");
                MessageBoxManager.GetMessageBoxStandard("Finish", "Finish without problems." ).ShowAsync();
            }
            else
            {
                _logger.LogInfo("Finish with errors:");
                foreach (var error in errors) _logger.LogWarning(error);
                MessageBoxManager.GetMessageBoxStandard("Finish with problems", "Finish with problems:\n" + string.Join("\n", errors)).ShowAsync();
            }
        }

        public bool HasCheckedCheckbox(Visual parent)
        {
            foreach (var child in parent.GetVisualChildren())
            {
                if (child is CheckBox cb && cb.IsChecked == true)
                    return true;

                if (child is Visual visual && HasCheckedCheckbox(visual))
                    return true;
            }

            return false;
        }

    }
}
