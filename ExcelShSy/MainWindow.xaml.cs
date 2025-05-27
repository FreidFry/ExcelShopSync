using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Properties;

using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ExcelShSy
{
    public partial class MainWindow : Window
    {
        private readonly ILocalizationService _localizationService;
        private readonly IFileManager _fileManager;
        private readonly ITaskFactory _taskFactory;

        public MainWindow(ILocalizationService localizationService, IFileManager fileManager, ITaskFactory taskFactory)
        {
            InitializeComponent();
            _localizationService = localizationService;
            _fileManager = fileManager;
            _taskFactory = taskFactory;
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

        private void StartDatePicker_SelectedDateChanged(object sender, EventArgs e)
        {
            var dp = sender as DatePicker;
            DateTime selectedDate = dp?.SelectedDate ?? DateTime.Today;
            DateOnly date = DateOnly.FromDateTime(selectedDate);
            GlobalSettings.DiscountStartDate = date;
        }

        private void EndDatePicker_SelectedDateChanged(object sender, EventArgs e)
        {
            var dp = sender as DatePicker;
            DateTime selectedDate = dp?.SelectedDate ?? DateTime.Today;
            DateOnly date = DateOnly.FromDateTime(selectedDate);
            GlobalSettings.DiscountEndDate = date;
        }
    }
}
