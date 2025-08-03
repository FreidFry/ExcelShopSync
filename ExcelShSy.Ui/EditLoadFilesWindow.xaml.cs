using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Events;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Ui.Models.EditLoadFiles;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

using static ExcelShSy.Ui.Localization.GetLocalizationInCode;

namespace ExcelShSy
{
    public partial class EditLoadFilesWindow : Window
    {
        public ObservableCollection<FileItem> TemperaryTargetFiles { get; set; } = [];
        public ObservableCollection<FileItem> TemperarySourceFiles { get; set; } = [];
        private readonly IFileManager _fileManager;
        private readonly IFileProvider _fileProvider;
        private readonly IExcelFileFactory _excelfileFactory;

        public EditLoadFilesWindow(string TabName, IFileManager fileManager, IFileProvider fileProvider, IExcelFileFactory fileFactory)
        {
            InitializeComponent();
            _fileManager = fileManager;
            _fileProvider = fileProvider;
            _excelfileFactory = fileFactory;

            DataContext = this;

            SetFileNameList(TemperaryTargetFiles, _fileManager.TargetPath);
            SetFileNameList(TemperarySourceFiles, _fileManager.SourcePath);

            SelectTab(TabName);
        }

        private void SelectTab(string TabName)
        {
            FilesControl.SelectedItem = TabName switch
            {
                "Target" => Target,
                "Source" => Source,
                _ => 0,
            };
        }

        private static void SetFileNameList(ObservableCollection<FileItem> observableCollection, IEnumerable<string> list)
        {
            foreach (string filePath in list)
            {
                var item = new FileItem(filePath);
                observableCollection.Add(item);
            }
        }

        private void AddFile_Click(object sender, EventArgs e)
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

        private void AddItems(ObservableCollection<FileItem> items)
        {
            var sources = _fileProvider.GetPaths();
            if (sources.IsNullOrEmpty()) return;
            foreach (var file in sources)
                if (!items.Any(item => item.FilePath == file))
                    items.Add(new FileItem(file));
        }

        private void RemoveFile_Click(object sender, EventArgs e)
        {
            var message = GetLocalizate("EditLoadFilesWindow", "DeleteWarning_");
            var title = GetLocalizate("EditLoadFilesWindow", "DeleteWarningTitle_");
            var remove = CreateMessageBoxYesNoWarning(message, title);
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

        private void RemoveItems(ObservableCollection<FileItem> items)
        {
            var remove = items.Where(item => item.IsSelectedToRemove).ToList();
            foreach (var file in remove)
            {
                items.Remove(file);
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            var message = GetLocalizate("EditLoadFilesWindow", "CloseWarning_");
            var title = GetLocalizate("EditLoadFilesWindow", "CloseWarningTitle_");
            var succes = CreateMessageBoxYesNoWarning(message, title);
            if (!succes) Close();
        }

        private bool CreateMessageBoxYesNoWarning(string message, string windowName)
        {
            var result = MessageBox.Show(message, windowName, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No) return true;
            return false;
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            TransferFileList();
            Close();
        }

        private void TransferFileList()
        {
            var targetList = TemperaryTargetFiles.Select(i => i.FilePath).ToList();
            var sourceList = TemperarySourceFiles.Select(i => i.FilePath).ToList();
            TextBlockEvents.UpdateText("TargetLb", "");
            TextBlockEvents.UpdateText("SourceLb", "");

            _fileManager.TargetPath.Clear();
            if (targetList != null && targetList.Count > 0)
            {
                _fileManager.TargetPath.AddRange(targetList);
                FileNameExtensions.SetLastPath("TargetLb", targetList);
            }
            _fileManager.SourcePath.Clear();
            if (sourceList != null && sourceList.Count > 0)
            {
                _fileManager.SourcePath.AddRange(sourceList);
                FileNameExtensions.SetLastPath("SourceLb", sourceList);
            }
        }

        public void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is FileItem target)
            {
                var path = target.FilePath;
                var file = _excelfileFactory.Create(path);
                file.ShowInfo();
            }
        }
    }
}
