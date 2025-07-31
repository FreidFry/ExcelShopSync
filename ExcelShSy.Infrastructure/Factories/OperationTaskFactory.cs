using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Infrastructure.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        public void ExecuteOperations(DependencyObject parent)
        {
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
                _logger.LogInfo($"Finish {task.GetType().Name}");

            }
            _logger.LogInfo("Finish");
            MessageBox.Show("Finish");
        }

        public bool IsAnyCheckboxChecked(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is CheckBox cb && cb.IsChecked == true)
                    return true;

                if (IsAnyCheckboxChecked(child))
                    return true;
            }
            return false;
        }
    }
}
