﻿using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Infrastructure.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using Avalonia;
using MsBox.Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using static ExcelShSy.Localization.GetLocalizationInCode;

namespace ExcelShSy.Infrastructure.Factories
{
    public class OperationTaskFactory : IOperationTaskFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly Dictionary<string, Type> _tasksMap;

        public OperationTaskFactory(IServiceProvider services, ILocalizationService localizationService, ILogger logger)
        {
            _serviceProvider = services;
            _localizationService = localizationService;
            _logger = logger;

            var allTasks = _serviceProvider.GetServices<IExecuteOperation>();

            _tasksMap = allTasks
                .Select(t => t.GetType())
                .Where(t => t.GetCustomAttribute<TaskAttribute>() != null)
                .ToDictionary(
                    t => t.GetCustomAttribute<TaskAttribute>()!.Name,
                    t => t);
        }

        public IExecuteOperation? CreateTask(string taskName)
        {
            try
            {
                _tasksMap.TryGetValue(taskName, out var taskType);
                return (IExecuteOperation?)_serviceProvider.GetRequiredService(taskType!);
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
                var title = _localizationService.GetMessageString("Finish");
                var msg = _localizationService.GetMessageString("FinishText");
                MessageBoxManager.GetMessageBoxStandard(title, msg).ShowAsync();
            }
            else
            {
                _logger.LogInfo("Finish with errors:");
                var title = _localizationService.GetMessageString("FinishWithProblems");
                var msg = _localizationService.GetMessageString("FinishWithProblemsText");
                foreach (var error in errors) _logger.LogWarning(error);
                MessageBoxManager.GetMessageBoxStandard(title, $"{msg}\n" + string.Join("\n", errors)).ShowAsync();
            }
        }

        public bool HasCheckedCheckbox(Visual parent)
        {
            foreach (var child in parent.GetVisualChildren())
            {
                switch (child)
                {
                    case CheckBox { IsChecked: true }:
                    case not null when HasCheckedCheckbox(child):
                        return true;
                }
            }
            return false;
        }

    }
}
