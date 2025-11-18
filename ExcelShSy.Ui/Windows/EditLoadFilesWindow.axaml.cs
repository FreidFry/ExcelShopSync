using Avalonia.Controls;
using Avalonia.Interactivity;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Core.Enums;
using ExcelShSy.Event;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Ui.Models.EditLoadFiles;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using System.Collections.ObjectModel;

namespace ExcelShSy.Ui.Windows
{
    public partial class EditLoadFilesWindow : Window
    {
        public ObservableCollection<ExcelFileItem> TemporaryTargetFiles { get; set; } = [];
        public ObservableCollection<ExcelFileItem> TemporarySourceFiles { get; set; } = [];
        private readonly IFileManager _fileManager;
        private readonly IFileProvider _fileProvider;
        private readonly IExcelFileFactory _excelFileFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IMessages<IMsBox<ButtonResult>> _messages;

#if DESIGNER
        public EditLoadFilesWindow()
        {
            InitializeComponent();
        }
#endif
        
        public EditLoadFilesWindow(IFileManager fileManager, IFileProvider fileProvider,
            IExcelFileFactory fileFactory, ILocalizationService localizationService, IMessages<IMsBox<ButtonResult>> messages)
            : this("Target", fileManager, fileProvider, fileFactory, localizationService, messages) { }
        public EditLoadFilesWindow(string tabName, IFileManager fileManager, IFileProvider fileProvider, IExcelFileFactory fileFactory, ILocalizationService localizationService, IMessages<IMsBox<ButtonResult>> messages)
        {
            InitializeComponent();
            
            _fileManager = fileManager;
            _fileProvider = fileProvider;
            _excelFileFactory = fileFactory;
            _localizationService = localizationService;
            _messages = messages;

            SetFileNameList(TemporaryTargetFiles, _fileManager.TargetPaths);
            SetFileNameList(TemporarySourceFiles, _fileManager.SourcePaths);
            DataContext = this;

            SelectTab(tabName);
        }

        #region Actions

        private void SelectTab(string tabName)
        {
            foreach (var item in FilesControl.Items)
            {
                if (item is not TabItem tab) continue;
                
                if (tab.Header?.ToString() != tabName) continue;
                FilesControl.SelectedItem = tab;
            }
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tag = button!.Tag as string;
            switch (tag)
            {
                case "Target":
                    AddItems(TemporaryTargetFiles);
                    break;
                case "Source":
                    AddItems(TemporarySourceFiles);
                    break;
            }
        }
        
        private async void RemoveFile_Click(object? sender, RoutedEventArgs e)
        {
            var message = _localizationService.GetString("EditLoadFilesWindow", "DeleteWarning_");
            var title = _localizationService.GetString("EditLoadFilesWindow", "DeleteWarningTitle_");
            var remove = await CreateMessageBoxYesNoWarning(message, title);
            if (remove) return;
            var button = sender as Button;
            var tag = button!.Tag as string;
            switch (tag)
            {
                case "Target":
                    RemoveItems(TemporaryTargetFiles);
                    break;
                case "Source":
                    RemoveItems(TemporarySourceFiles);
                    break;
            }
        }
        
        private async void Close_Click(object sender, RoutedEventArgs e)
        {
            var message = _localizationService.GetString("EditLoadFilesWindow", "CloseWarning_");
            var title = _localizationService.GetString("EditLoadFilesWindow", "CloseWarningTitle_");
            var yesNoWarning = await CreateMessageBoxYesNoWarning(message, title);
            if (!yesNoWarning) Close();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            TransferFileList();
            Close();
        }

        public void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button { DataContext: ExcelFileItem target }) return;
            
            var path = target.FilePath;
            var file = _excelFileFactory.Create(path);
            file.ShowFileDetails();
        }
        
        #endregion

        #region Inner methods

        
        private static void SetFileNameList(ObservableCollection<ExcelFileItem> observableCollection, IEnumerable<string> list)
        {
            foreach (var filePath in list)
            {
                var item = new ExcelFileItem(filePath);
                observableCollection.Add(item);
            }
        }

        private async void AddItems(ObservableCollection<ExcelFileItem> items)
        {
            var sources = await _fileProvider.PickExcelFilePaths();
            if (sources.IsNullOrEmpty()) return;
            foreach (var file in sources.Where(file => items.All(item => item.FilePath != file)))
                items.Add(new ExcelFileItem(file));
        }

        private static void RemoveItems(ObservableCollection<ExcelFileItem> items)
        {
            var remove = items.Where(item => item.IsSelectedToRemove).ToList();
            foreach (var file in remove)
            {
                items.Remove(file);
            }
        }

        private async Task<bool> CreateMessageBoxYesNoWarning(string message, string windowName)
        {
            var msBox = _messages.GetMessageBoxStandard(windowName, message, Core.Enums.MyButtonEnum.YesNo, Core.Enums.MyIcon.Warning);
            var result = await msBox.ShowAsync();
            return result == ButtonResult.No;
        }

        private void TransferFileList()
        {
            var targetList = TemporaryTargetFiles.Select(i => i.FilePath).ToList();
            var sourceList = TemporarySourceFiles.Select(i => i.FilePath).ToList();
            UpdateTextBlockEvents.UpdateText("TargetLb", "");
            UpdateTextBlockEvents.UpdateText("SourceLb", "");

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

        #endregion
    }
}
