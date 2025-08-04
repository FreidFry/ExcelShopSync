using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Events;
using ExcelShSy.Properties;
using ExcelShSy.Ui.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public MainWindow(IFileManager fileManager, IOperationTaskFactory taskFactory, ILogger logger, IEditLoadFilesWindowFactory editLoadFilesWindowFactory, ISettingWindowFactory settingWindowFactory)
        {
            InitializeComponents();
            _fileManager = fileManager;
            _taskFactory = taskFactory;
            _logger = logger;
            _editLoadFilesWindowFactory = editLoadFilesWindowFactory;
            _settingWindowFactory = settingWindowFactory;

            RegestrationTextBlockEvent("TargetLb", GetTargetFileLable);
            RegestrationTextBlockEvent("SourceLb", GetSourceFileLable);
        }

        private void InitializeComponents()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                var twight = TargetFilesButton.ActualWidth;
                var swight = SourceFilesButton.ActualWidth;

                if (swight > twight)
                    TargetFilesButton.Width = swight;
                else
                    SourceFilesButton.Width = twight;
            };
        }

        private static void RegestrationTextBlockEvent(string key, TextBlock textBlock)
        {
            TextBlockEvents.OnTextUpdate += (tarketKey, text) =>
            {
                if (tarketKey == key)
                    textBlock.Text = text;
            };
        }

        private void GetTargetFile_Click(object sender, RoutedEventArgs e)
        {
            _fileManager.AddTargetFilesPath();
        }

        private void GetSourceFile_Click(object sender, RoutedEventArgs e)
        {
            _fileManager.AddSourceFilesPath();
        }

        private void ExecuteTasks_Click(object sender, RoutedEventArgs e)
        {
            if (!_taskFactory.IsAnyCheckboxChecked(TaskGrid))
            {
                var message = GetLocalizate("MainWindow", "NoSetExecute_");
                var title = GetLocalizate("MainWindow", "NoSetExecuteTitle_");
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _fileManager.InitializeFiles();
            _taskFactory.ExecuteOperations(TaskGrid);
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            var propertyName = cb?.Tag?.ToString();
            var value = cb?.IsChecked == true;

            if (!string.IsNullOrEmpty(propertyName))
            {
                var settingsType = typeof(GlobalSettings);
                var prop = settingsType.GetProperty(propertyName);
                prop?.SetValue(null, value);
            }
        }

        private void ChangeIncreasePercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal.TryParse(IncreasePercentTextBox.Text, out decimal percents);
            GlobalSettings.priceIncreasePercentage = percents;
        }

        private void IncreasePercentTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text, ((TextBox)sender).Text);
        }

        private void IncreasePercentTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pastedText = (string)e.DataObject.GetData(typeof(string));
                var textBox = (TextBox)sender;
                string newText = textBox.Text.Insert(textBox.SelectionStart, pastedText);
                if (!IsTextAllowed(pastedText, newText))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private static bool IsTextAllowed(string newInput, string fullText)
        {
            string text = fullText + newInput;
            return decimal.TryParse(text, out _);
        }

        private void ShowEditLoadFiles_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var propertyName = btn?.Tag?.ToString();

            if (!string.IsNullOrEmpty(propertyName))
            {
                var window = _editLoadFilesWindowFactory.Create(propertyName);
                window.ShowDialog();
            }
        }

        private void SettingsWindowOpen_Click(object sender, EventArgs e)
        {
            OpenSettingWindow();
        }

        public void OpenSettingWindow()
        {
            var window = _settingWindowFactory.Create();
            window.ShowDialog();
        }

        private void AboutWindowOpen_Click(object sender, EventArgs e)
        {
            var window = new WPFAboutF4Labs.F4LabsAboutWindow();
            window.ShowDialog();
        }

        private void GuideWindowOpen_Click(object sender, EventArgs e)
        {
            var window = new Guide.WebView();
            window.Show();
        }
    }
}
