using System.Diagnostics;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Event;
using ExcelShSy.Ui.Interfaces;
using Avalonia.Controls;
using Avalonia.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Avalonia.Interactivity;
using ExcelShSy.Core.Properties;
using static System.Decimal;
using static ExcelShSy.Localization.GetLocalizationInCode;

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

        public MainWindow(IFileManager fileManager, IOperationTaskFactory taskFactory, ILogger logger, IEditLoadFilesWindowFactory editLoadFilesWindowFactory, ISettingWindowFactory settingWindowFactory, IDataBaseViewerFactory dataBaseViewer)
        {
            InitializeComponents();
            _fileManager = fileManager;
            _taskFactory = taskFactory;
            _logger = logger;
            _editLoadFilesWindowFactory = editLoadFilesWindowFactory;
            _settingWindowFactory = settingWindowFactory;
            _dataBaseViewer = dataBaseViewer;

            RegistrationTextBlockEvent("TargetLb", GetTargetFileLable);
            RegistrationTextBlockEvent("SourceLb", GetSourceFileLable);
        }

        private void InitializeComponents()
        {
            InitializeComponent();

            this.LayoutUpdated += (sender, args) =>
            {
                var tWidth = TargetFilesButton.Bounds.Width;
                var sWidth = SourceFilesButton.Bounds.Width;

                var max = Math.Max(tWidth, sWidth);
                TargetFilesButton.Width = max;
                SourceFilesButton.Width = max;
                
                this.LayoutUpdated -= null;
            };
        }

        private static void RegistrationTextBlockEvent(string key, TextBlock textBlock)
        {
            UpdateTextBlockEvents.OnTextUpdate += (targetKey, text) =>
            {
                if (targetKey == key)
                    textBlock.Text = text;
            };
        }

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
            if (!_taskFactory.HasCheckedCheckbox(TaskGrid))
            {
                var message = GetLocalizate("MainWindow", "NoSetExecute_");
                var title = GetLocalizate("MainWindow", "NoSetExecuteTitle_");
                var msBox = MessageBoxManager.GetMessageBoxStandard(title,message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                msBox.ShowAsync();
                return;
            }
            _fileManager.InitializeAllFiles();
            _taskFactory.ExecuteOperations(TaskGrid);
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

        private void ChangeIncreasePercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            TryParse(IncreasePercentTextBox.Text, out decimal percents);
            ProductProcessingOptions.priceIncreasePercentage = percents;
        }

        private void IncreasePercentTextBox_TextInput(object? sender, TextInputEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);
                if (!IsTextAllowed(e.Text, newText))
                    e.Handled = true; // отменяем ввод
            }
        }

        private void IncreasePercentTextBox_OnTextInput(object sender, TextInputEventArgs e)
        {
            var textBox = (TextBox)sender;
            string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);
    
            if (!IsTextAllowed(e.Text, newText))
                e.Handled = true; // отменяем ввод 
        }


        private static bool IsTextAllowed(string newInput, string fullText)
        {
            string text = fullText + newInput;
            return TryParse(text, out _);
        }

        private void ShowEditLoadFiles_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var propertyName = btn?.Tag?.ToString();

            if (!string.IsNullOrEmpty(propertyName))
            {
                var window = _editLoadFilesWindowFactory.Create(propertyName);
                window.ShowDialog(this);
            }
        }

        private void SettingsWindowOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenSettingWindow();
        }

        private async void OpenSettingWindow()
        {
            var window = _settingWindowFactory.Create();
            await window.ShowDialog(this);
        }

        private async void AboutWindowOpen_Click(object sender, RoutedEventArgs e)
        {
            var window = new WPFAboutF4Labs.F4LabsAboutWindow();
            await window.ShowDialog(this);
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
        
        private static string SelectGuidePage()
        {
            var language = Thread.CurrentThread.CurrentCulture.Name;
            const string fileName = "Guid";
            var fileDirectory = Path.Combine(Environment.CurrentDirectory, "Web");
            var path = Path.Combine(fileDirectory, $"{fileName}.{language}.html");
            var baseFile = Path.Combine(fileDirectory, $"{fileName}.html");

            return File.Exists(path) ? path : baseFile;
        }


        private async void OpenDbManager_Click(object? sender, RoutedEventArgs e)
        {
            var window = _dataBaseViewer.Create();
            await window.ShowDialog(this);
        }
    }
}
