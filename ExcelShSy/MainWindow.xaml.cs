using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Core.Services.Logger;
using ExcelShSy.Properties;

using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExcelShSy
{
    public partial class MainWindow : Window
    {
        private readonly ILocalizationService _localizationService;
        private readonly IFileManager _fileManager;
        private readonly ITaskFactory _taskFactory;
        private readonly ILogger _logger;

        public MainWindow(ILocalizationService localizationService, IFileManager fileManager, ITaskFactory taskFactory, ILogger logger)
        {
            InitializeComponent();
            _localizationService = localizationService;
            _fileManager = fileManager;
            _taskFactory = taskFactory;
            _logger = logger;
        }

        private void GetTargetFile_Click(object sender, RoutedEventArgs e)
        {
            _fileManager.AddTargetFilesPath(GetTargetFileLable);
        }

        private void GetSourceFile_Click(object sender, RoutedEventArgs e)
        {
            _fileManager.AddSourceFilesPath(GetSourceFileLable);
        }

        private void ExecuteTasks_Click(object sender, RoutedEventArgs e)
        {
            if (!_taskFactory.Validate(TaskGrid))
            {
                MessageBox.Show("Выберите задачу");
                return;
            }
            _fileManager.InitializeFiles();
            _taskFactory.RelizeExecute(TaskGrid);
        }

        private void ChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            _localizationService.SetCulture(_localizationService.CurrentCulture == new CultureInfo("uk-UA") ? "uk-UA" : "ru-RU");
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

        private bool IsTextAllowed(string newInput, string fullText)
        {
            string text = fullText + newInput;
            return decimal.TryParse(text, out _);
        }
    }
}
