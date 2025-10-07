using System.Diagnostics;
using System.Globalization;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Event;
using ExcelShSy.Ui.Interfaces;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Avalonia.Interactivity;
using ExcelShSy.Core.Properties;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace ExcelShSy.Ui
{
    public partial class MainWindow : Window
    {
        private readonly IFileManager _fileManager;
        private readonly IOperationTaskFactory _taskFactory;
        private readonly ILogger _logger;
        private readonly IEditLoadFilesWindowFactory _editLoadFilesWindowFactory;
        private readonly ISettingWindowFactory _settingWindowFactory;
        private readonly IDataBaseViewerFactory _dataBaseViewer;
        private readonly IF4LabsAboutWindowFactory _f4LabsAboutWindowFactory;
        private readonly ILocalizationService _localizationService;

        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(IFileManager fileManager, IOperationTaskFactory taskFactory, ILogger logger, IEditLoadFilesWindowFactory editLoadFilesWindowFactory, ISettingWindowFactory settingWindowFactory, IDataBaseViewerFactory dataBaseViewer,
           IF4LabsAboutWindowFactory f4LabsAboutWindowFactory,ILocalizationService localizationService)
        {
            InitializeComponent();
            
            _fileManager = fileManager;
            _taskFactory = taskFactory;
            _logger = logger;
            _editLoadFilesWindowFactory = editLoadFilesWindowFactory;
            _settingWindowFactory = settingWindowFactory;
            _dataBaseViewer = dataBaseViewer;
            _f4LabsAboutWindowFactory = f4LabsAboutWindowFactory;
            _localizationService = localizationService;

            UpdateTextBlockEvents.RegistrationTextBlockEvent("TargetLb", TargetLastFile);
            UpdateTextBlockEvents.RegistrationTextBlockEvent("SourceLb", SourceLastFile);
        }

        #region OtherWindows

        private void ShowEditLoadFiles_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var propertyName = btn?.Tag?.ToString();


            var window = string.IsNullOrEmpty(propertyName)
                ? _editLoadFilesWindowFactory.Create()
                : _editLoadFilesWindowFactory.Create(propertyName);
            window.ShowDialog(this);
        }

        private void SettingsWindowOpen_Click(object sender, RoutedEventArgs e)
        {
            var window = _settingWindowFactory.Create();
            window.ShowDialog(this);
        }

        private void AboutWindowOpen_Click(object sender, RoutedEventArgs e)
        {
            var window = _f4LabsAboutWindowFactory.Create();
            window.ShowDialog(this);
        }
        
        private void OpenDbManager_Click(object? sender, RoutedEventArgs e)
        {
            var window = _dataBaseViewer.Create();
            window.ShowDialog(this);
        }
        
        private void GuideWindowOpen_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo()
            {
                FileName = SelectGuidePage(),
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        
        #region Support

        private static string SelectGuidePage()
        {
            var language = Thread.CurrentThread.CurrentCulture.Name;
            const string fileName = "Guid";
            var fileDirectory = Path.Combine(Environment.CurrentDirectory, "Web");
            var path = Path.Combine(fileDirectory, $"{fileName}.{language}.html");
            var baseFile = Path.Combine(fileDirectory, $"{fileName}.html");

            return File.Exists(path) ? path : baseFile;
        }

        #endregion

        #endregion

        #region Actions

        private void GetTargetFile_Click(object sender, RoutedEventArgs e)
        {
            _fileManager.AddTargetPath();
        }

        private void GetSourceFile_Click(object sender, RoutedEventArgs e)
        {
            _fileManager.AddSourcePath();
        }

        private void ExecuteTasks_Click(object sender, RoutedEventArgs e)
        {
            if (!_taskFactory.HasCheckedCheckbox(TasksContainer))
            {
                var message = _localizationService.GetString("MainWindow", "NoSetExecute_");
                var title = _localizationService.GetString("MainWindow", "NoSetExecuteTitle_");
                var msBox = MessageBoxManager.GetMessageBoxStandard(title,message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                msBox.ShowAsync();
                return;
            }
            _fileManager.InitializeAllFiles();
            _taskFactory.ExecuteOperations(TasksContainer);
            _fileManager.ClearAfterComplete();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            var propertyName = cb?.Tag?.ToString();
            var value = cb?.IsChecked == true;

            if (!string.IsNullOrEmpty(propertyName))
            {
                var settingsType = typeof(ProductProcessingOptions);
                var prop = settingsType.GetProperty(propertyName);
                prop?.SetValue(null, value);
            }
        }

        #endregion

        #region Events

        private void ChangeIncreasePercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox textBox)
                return;
            
            var filtered = IsTextAllowed(textBox.Text);

            if (filtered != textBox.Text)
            {
                textBox.Text = filtered;
                textBox.CaretIndex = filtered.Length;
                return;
            }
            if (decimal.TryParse(filtered, NumberStyles.Number, CultureInfo.InvariantCulture, out var percents))
                ProductProcessingOptions.priceIncreasePercentage = percents;
        }
        
        #endregion

        private static string IsTextAllowed(string? newInput) =>
            new(newInput?.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray());
    }
}
