using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Properties;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;
using System.Windows;

namespace ExcelShSy
{
    public partial class MainWindow : Window
    {
        private readonly ILocalizationService _localizationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileManager _fileManager;
        private readonly ITaskFactory _taskFactory;

        public MainWindow(ILocalizationService localizationService, IServiceProvider serviceProvider, IFileManager fileManager, ITaskFactory taskFactory)
        {
            InitializeComponent();
            _localizationService = localizationService;
            _serviceProvider = serviceProvider;
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
            _fileManager.InitializeFiles(FromPriceListCheckBox.IsChecked);
            _taskFactory.RelizeExecute(TaskGrid);
        }

        private void IsRound_Checked(object sender, RoutedEventArgs e) => GlobalSettings.IsRound = true;

        private void IsRound_Unchecked(object sender, RoutedEventArgs e) => GlobalSettings.IsRound = false;

        private void ChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            _localizationService.SetCulture(_localizationService.CurrentCulture == new CultureInfo("uk-UA") ? "uk-UA" : "ru-RU");
        }
    }
}
