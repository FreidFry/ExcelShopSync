using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Events;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Ui.Models.EditLoadFiles;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using static ExcelShSy.Localization.GetLocalizationInCode;

namespace ExcelShSy.Ui
{
    public partial class EditLoadFilesWindow : Window
    {
        public ObservableCollection<ExcelFileItem> TemperaryTargetFiles { get; set; } = [];
        public ObservableCollection<ExcelFileItem> TemperarySourceFiles { get; set; } = [];
        private readonly IFileManager _fileManager;
        private readonly IFileProvider _fileProvider;
        private readonly IExcelFileFactory _excelFileFactory;
        
        public EditLoadFilesWindow(string tabName, IFileManager fileManager, IFileProvider fileProvider, IExcelFileFactory fileFactory)
        {
            InitializeComponent();
            
            _fileManager = fileManager;
            _fileProvider = fileProvider;
            _excelFileFactory = fileFactory;


            SetFileNameList(TemperaryTargetFiles, _fileManager.TargetPaths);
            SetFileNameList(TemperarySourceFiles, _fileManager.SourcePaths);
            DataContext = this;

            SelectTab(tabName);
        }

        private void SelectTab(string tabName)
        {
            foreach (var item in FilesControl.Items)
            {
                if (item is TabItem tab && tab.Header?.ToString() == tabName)
                {
                    FilesControl.SelectedItem = tab;
                    break;
                }
            }
        }
        
        private static void SetFileNameList(ObservableCollection<ExcelFileItem> observableCollection, IEnumerable<string> list)
        {
            foreach (string filePath in list)
            {
                var item = new ExcelFileItem(filePath);
                observableCollection.Add(item);
            }
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tag = button!.Tag as string;
            switch (tag)
            {
                case "Target":
                    AddItems(TemperaryTargetFiles);
                    break;
                case "Source":
                    AddItems(TemperarySourceFiles);
                    break;
                default: break;
            }
        }

        private void AddItems(ObservableCollection<ExcelFileItem> items)
        {
            var sources = _fileProvider.PickExcelFilePaths();
            if (sources.Result.IsNullOrEmpty()) return;
            foreach (var file in sources.Result)
                if (!items.Any(item => item.FilePath == file))
                    items.Add(new ExcelFileItem(file));
        }

        private async void RemoveFile_Click(object? sender, RoutedEventArgs e)
        {
            var message = GetLocalizate("EditLoadFilesWindow", "DeleteWarning_");
            var title = GetLocalizate("EditLoadFilesWindow", "DeleteWarningTitle_");
            var remove = await CreateMessageBoxYesNoWarning(message, title);
            if (remove) return;
            var button = sender as Button;
            var tag = button!.Tag as string;
            switch (tag)
            {
                case "Target":
                    RemoveItems(TemperaryTargetFiles);
                    break;
                case "Source":
                    RemoveItems(TemperarySourceFiles);
                    break;
                default: break;
            }
        }

        private void RemoveItems(ObservableCollection<ExcelFileItem> items)
        {
            var remove = items.Where(item => item.IsSelectedToRemove).ToList();
            foreach (var file in remove)
            {
                items.Remove(file);
            }
        }

        private async void Close_Click(object sender, RoutedEventArgs e)
        {
            var message = GetLocalizate("EditLoadFilesWindow", "CloseWarning_");
            var title = GetLocalizate("EditLoadFilesWindow", "CloseWarningTitle_");
            var succes = await CreateMessageBoxYesNoWarning(message, title);
            if (!succes) Close();
        }

        private async Task<bool> CreateMessageBoxYesNoWarning(string message, string windowName)
        {
            var msBox = MessageBoxManager.GetMessageBoxStandard(windowName, message, ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Warning);
            var result = await msBox.ShowAsync();
            if (result == ButtonResult.No) return true;
            return false;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            TransferFileList();
            Close();
        }

        private void TransferFileList()
        {
            var targetList = TemperaryTargetFiles.Select(i => i.FilePath).ToList();
            var sourceList = TemperarySourceFiles.Select(i => i.FilePath).ToList();
            UpdateTextBlockEvents.UpdateText("TargetLb", "");
            UpdateTextBlockEvents.UpdateText("SourceLb", "");

            _fileManager.TargetPaths.Clear();
            if (targetList != null && targetList.Count > 0)
            {
                _fileManager.TargetPaths.AddRange(targetList);
                FileNameExtensions.SetLastPath("TargetLb", targetList);
            }
            _fileManager.SourcePaths.Clear();
            if (sourceList != null && sourceList.Count > 0)
            {
                _fileManager.SourcePaths.AddRange(sourceList);
                FileNameExtensions.SetLastPath("SourceLb", sourceList);
            }
        }

        public void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ExcelFileItem target)
            {
                var path = target.FilePath;
                var file = _excelFileFactory.Create(path);
                file.ShowFileDetails();
            }
        }
    }
}
