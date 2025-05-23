using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.UiUtils;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using System.Windows.Controls;

namespace ExcelShSy.Core.Factorys
{
    public class MyTaskFactory : ITaskFactory
    {
        readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> _tasksMap;

        public MyTaskFactory(IServiceProvider services)
        {
            _serviceProvider = services;

            var allTasks = _serviceProvider.GetServices<IExecuteOperation>();

            _tasksMap = allTasks
                .Select(t => t.GetType())
                .Where(t => t.GetCustomAttribute<TaskAttribute>() != null)
                .ToDictionary(
                    t => t.GetCustomAttribute<TaskAttribute>().Name,
                    t => t);
        }

        public IExecuteOperation CreateTask(string taskName)
        {
            if (!_tasksMap.TryGetValue(taskName, out var taskType))
                throw new ArgumentException($"Unknown task {taskName}");

            return (IExecuteOperation)_serviceProvider.GetRequiredService(taskType);
        }

        public void RelizeExecute(Grid TaskGrid)
        {
            List<IExecuteOperation> tasksToRun = [];
            tasksToRun.GetExecuteTask(TaskGrid, this);

            tasksToRun.Add(CreateTask("SavePackages"));
            foreach (var task in tasksToRun)
            {
                task.Execute();
            }
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
