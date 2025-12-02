using ExcelShSy.Core.Enums;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Core.Interfaces.ViewModels;
using ExcelShSy.Event;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Ui.Models.EditLoadFiles;
using ExcelShSy.Ui.ModelView.Base;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using System.Collections.ObjectModel;

namespace ExcelShSy.Ui.ModelView.View
{
    public class EditLoadFilesModel : ViewModelBase, IEditLoadFilesModel
    {
        private readonly IFileManager _fileManager;
        private readonly IFileProvider _fileProvider;
        private readonly IExcelFileFactory _excelFileFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IMessages<IMsBox<ButtonResult>> _messages;

        public EditLoadFilesModel(IFileManager fileManager, IFileProvider fileProvider, IExcelFileFactory fileFactory, ILocalizationService localizationService, IMessages<IMsBox<ButtonResult>> messages)
        {
            _fileManager = fileManager;
            _fileProvider = fileProvider;
            _excelFileFactory = fileFactory;
            _localizationService = localizationService;
            _messages = messages;

            ApplyAsyncCommand = new AsyncRelayCommands(async _ => await ApplyAsync());
            ShowInfoCommand = new AsyncRelayCommands(async path =>
            {
                if (path is not string filePath) return;
                await ShowInfoFromPath(filePath);
            });
            AddFileCommand = new AsyncRelayCommands(async e =>
            {
                if (e is not FileTagEnum tag) return;
                await AddFile(tag);
            });
            RemoveFileCommand = new AsyncRelayCommands(async e =>
            {
                if (e is not FileTagEnum tag) return;
                await RemoveFile(tag);
            });

            CancelCommand = new AsyncRelayCommands(async _ => await Cancel());

            LoadFiles(TargetFiles, _fileManager.TargetPaths, ShowInfoCommand);
            LoadFiles(SourceFiles, _fileManager.SourcePaths, ShowInfoCommand);
        }

        #region Fields

        public ObservableCollection<ExcelFileItem> TargetFiles { get; } = [];
        public ObservableCollection<ExcelFileItem> SourceFiles { get; } = [];

        #endregion

        private static void LoadFiles(ObservableCollection<ExcelFileItem> observableCollection, IEnumerable<string> list, AsyncRelayCommands showInfoCommand)
        {
            foreach (var filePath in list)
            {
                var item = new ExcelFileItem(filePath);
                item.ShowInfoCommand = showInfoCommand;
                observableCollection.Add(item);
            }
        }

        private async Task AddItems(ObservableCollection<ExcelFileItem> items)
        {
            var sources = await _fileProvider.PickExcelFilePaths();
            if (sources.IsNullOrEmpty()) return;
            foreach (var file in sources.Where(file => items.All(item => item.FilePath != file)))
                items.Add(new ExcelFileItem(file));
        }

        private async Task<bool> CreateMessageBoxYesNoWarning(string message, string windowName)
        {
            var msBox = _messages.GetMessageBoxStandard(windowName, message, Core.Enums.MyButtonEnum.YesNo, Core.Enums.MyIcon.Warning);
            var result = await msBox.ShowAsync();
            return result == ButtonResult.No;
        }

        private static async Task RemoveItems(ObservableCollection<ExcelFileItem> items)
        {
            var remove = items.Where(i => i.IsSelectedToRemove).ToList();
            foreach (var file in remove)
                items.Remove(file);
        }

        #region Relay Commands

        public AsyncRelayCommands ApplyAsyncCommand { get; }

        private async Task ApplyAsync()
        {
            var targetList = TargetFiles.Select(i => i.FilePath).ToList();
            var sourceList = SourceFiles.Select(i => i.FilePath).ToList();
            UpdateTextBlockEvents.UpdateText("TargetLb", string.Empty);
            UpdateTextBlockEvents.UpdateText("SourceLb", string.Empty);

            _fileManager.TargetPaths.Clear();
            if (targetList.Count > 0)
            {
                _fileManager.TargetPaths.AddRange(targetList);
                FileNameExtensions.SetLastPath("TargetLb", targetList);
            }
            _fileManager.SourcePaths.Clear();
            if (sourceList.Count > 0)
            {
                _fileManager.SourcePaths.AddRange(sourceList);
                FileNameExtensions.SetLastPath("SourceLb", sourceList);
            }
        }

        public AsyncRelayCommands ShowInfoCommand { get; }

        public async Task ShowInfoFromPath(string excelFilePath)
        {
            var file = _excelFileFactory.Create(excelFilePath);
            await file.ShowFileDetails();
        }

        public AsyncRelayCommands AddFileCommand { get; }
        private async Task AddFile(FileTagEnum tag)
        {
            switch (tag)
            {
                case FileTagEnum.Target:
                    await AddItems(TargetFiles);
                    break;
                case FileTagEnum.Source:
                    await AddItems(SourceFiles);
                    break;
            }
        }

        public AsyncRelayCommands RemoveFileCommand { get; }
        private async Task RemoveFile(FileTagEnum tag)
        {
            var message = _localizationService.GetString("EditLoadFilesWindow", "DeleteWarning_");
            var title = _localizationService.GetString("EditLoadFilesWindow", "DeleteWarningTitle_");
            var remove = await CreateMessageBoxYesNoWarning(message, title);
            if (remove) return;
            switch (tag)
            {
                case FileTagEnum.Target:
                    await RemoveItems(TargetFiles);
                    break;
                case FileTagEnum.Source:
                    await RemoveItems(SourceFiles);
                    break;
            }
        }

        public AsyncRelayCommands CancelCommand { get; }

        private async Task Cancel()
        {
            var message = _localizationService.GetString("EditLoadFilesWindow", "CloseWarning_");
            var title = _localizationService.GetString("EditLoadFilesWindow", "CloseWarningTitle_");
            var yesNoWarning = await CreateMessageBoxYesNoWarning(message, title);
        }

        #endregion
    }
}
