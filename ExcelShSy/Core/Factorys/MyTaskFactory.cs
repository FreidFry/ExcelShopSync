using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Services.Logger;
using ExcelShSy.UiUtils;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace ExcelShSy.Core.Factorys
{
    public class MyTaskFactory : ITaskFactory
    {
        readonly IServiceProvider _serviceProvider;
        readonly ILogger _logger;
        private readonly Dictionary<string, Type> _tasksMap;

        public MyTaskFactory(IServiceProvider services, ILogger logger)
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

        public void RelizeExecute(Grid TaskGrid)
        {
            List<IExecuteOperation> tasksToRun = [];
            tasksToRun.GetExecuteTask(TaskGrid, this);
            _logger.LogInfo("Executes: " + string.Join(", ", tasksToRun));
            var _ = CreateTask("SavePackages");
            if (_ != null)
                tasksToRun.Add(_);
            foreach (var task in tasksToRun)
            {
                _logger.LogInfo($"Start {task.GetType().Name}");
                task.Execute();
                _logger.LogInfo($"Finish {task.GetType().Name}");

            }
            _logger.LogInfo("Finish");
            MessageBox.Show("Finish");
        }

        public bool Validate(Grid TaskGrid)
        {

            foreach (var child in TaskGrid.Children)
                if (child is CheckBox cb && cb.IsChecked == true)
                    return true;
            return false;
        }
    }
}
