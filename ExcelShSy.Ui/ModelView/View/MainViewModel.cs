using Avalonia.Controls;
using Avalonia.Interactivity;
using ExcelShSy.Core.Enums;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Core.Interfaces.ViewModels;
using ExcelShSy.Core.Properties;
using ExcelShSy.LocalDataBaseModule;
using ExcelShSy.Ui.Interfaces;
using ExcelShSy.Ui.ModelView.Base;
using ExcelShSy.Ui.Windows;
using System.Diagnostics;
using System.Globalization;
using System.Reactive.Linq;
using WPFAboutF4Labs;

namespace ExcelShSy.Ui.ModelView.View
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly IAppSettings _appSettings;
        private readonly IFileManager _fileManager;
        private readonly IOperationTaskFactory _taskFactory;
        private readonly IEditLoadFilesWindowFactory _editLoadFilesWindowFactory;
        private readonly IWindowFactory<SettingWindow> _settingWindowFactory;
        private readonly IWindowFactory<DataBaseViewer> _dataBaseViewer;
        private readonly IWindowFactory<F4LabsAboutWindow> _f4LabsAboutWindowFactory;
        private readonly ICheckConnectionFactory _checkConnectionFactory;
        private readonly IUpdateManagerFactory _updateManagerFactory;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly IDialogService _dialogService;
        private readonly IWindowProvider _windowProvider;


        public MainViewModel(IAppSettings appSettings, IFileManager fileManager, IOperationTaskFactory taskFactory, IEditLoadFilesWindowFactory editLoadFilesWindowFactory, IWindowFactory<SettingWindow> settingWindowFactory, IWindowFactory<DataBaseViewer> dataBaseViewer,
           IWindowFactory<F4LabsAboutWindow> f4LabsAboutWindowFactory, ICheckConnectionFactory checkConnectionFactory, IUpdateManagerFactory updateManagerFactory, ILocalizationService localizationService, ILogger logger, IDialogService dialogService, IWindowProvider windowProvider)
        {
            _appSettings = appSettings;
            _fileManager = fileManager;
            _taskFactory = taskFactory;
            _editLoadFilesWindowFactory = editLoadFilesWindowFactory;
            _settingWindowFactory = settingWindowFactory;
            _dataBaseViewer = dataBaseViewer;
            _f4LabsAboutWindowFactory = f4LabsAboutWindowFactory;
            _checkConnectionFactory = checkConnectionFactory;
            _updateManagerFactory = updateManagerFactory;
            _localizationService = localizationService;
            _logger = logger;
            _dialogService = dialogService;
            _windowProvider = windowProvider;

            #region Initialize Commands

            ShowDbManagerCommand = new RelayCommands(_ => ShowDbManager());
            ShowAboutCommand = new RelayCommands(_ => ShowAboutWindow());
            ShowEditLoadFilesCommand = new RelayCommands(_ => ShowEditLoadFiles());
            ShowSettingsCommand = new RelayCommands(_ => ShowSettingsWindow());
            ShowGuideCommand = new RelayCommands(_ => GuideWindowOpen());
            GetTargetFilesCommand = new RelayCommands(_ => GetTargetFiles());
            GetSourceFilesCommand = new RelayCommands(_ => GetSourceFiles());
            CheckForUpdatesCommand = new AsyncRelayCommands(async _ => await CheckForUpdates());
            ExecuteTasksCommand = new AsyncRelayCommands(async _ => await ExecuteTasks());
            CheckBoxChanged = new RelayCommands(param =>
            {
                if (param is not CheckBox checkBox) return;
                CheckBoxChange(checkBox.IsChecked, checkBox.Tag?.ToString());
            });

            #endregion
        }

        #region Fields

        private int _activeExecutes;
        private string _increasePercentTextBox = string.Empty;
        private List<string> _tasksToRun = new();

        #endregion

        #region Properties

        private int ActiveExecutes { get; set; }
        public string IncreasePercentTextBox
        {
            get => _increasePercentTextBox;
            set
            {
                SetProperty(ref _increasePercentTextBox, value);
            }
        }
        public List<string> TasksToRun
        {
            get => _tasksToRun;
            set
            {
                SetProperty(ref _tasksToRun, value);
            }
        }

        #endregion

        #region Relay Commands

        public RelayCommands ShowEditLoadFilesCommand { get; }

        public void ShowEditLoadFiles()
        {
            var owner = _windowProvider.GetActiveWindow();

            var window = _editLoadFilesWindowFactory.Create();
            if (owner != null) window.ShowDialog(owner);
            else window.Show();
        }

        public RelayCommands ShowSettingsCommand { get; }
        public void ShowSettingsWindow()
        {
            var owner = _windowProvider.GetActiveWindow();
            var window = _settingWindowFactory.Create();
            if (owner != null) window.ShowDialog(owner);
            else window.Show();
        }

        public RelayCommands ShowAboutCommand { get; }
        public void ShowAboutWindow()
        {
            var owner = _windowProvider.GetActiveWindow();
            var window = _f4LabsAboutWindowFactory.Create();
            if (owner != null) window.ShowDialog(owner);
            else window.Show();
        }

        public RelayCommands ShowDbManagerCommand { get; }
        public void ShowDbManager()
        {
            var owner = _windowProvider.GetActiveWindow();
            var window = _dataBaseViewer.Create();
            if (owner != null) window.ShowDialog(owner);
            else window.Show();
        }

        public RelayCommands ShowGuideCommand { get; }
        private void GuideWindowOpen()
        {
            var psi = new ProcessStartInfo()
            {
                FileName = SelectGuidePage(),
                UseShellExecute = true
            };
            try
            {
                Process.Start(psi);
            }
            catch
            {
                _dialogService.ShowDefaultDialogAsync("Error", "Cannot open guide page.");
                _logger.LogError($"Guide files not found. (maybe moved or delete) exist status: {psi.FileName} {File.Exists(psi.FileName)}");
            }
        }

        public RelayCommands GetTargetFilesCommand { get; }
        public void GetTargetFiles()
        {
            _fileManager.AddTargetPath();
        }

        public RelayCommands GetSourceFilesCommand { get; }
        public void GetSourceFiles()
        {
            _fileManager.AddSourcePath();
        }

        public AsyncRelayCommands CheckForUpdatesCommand { get; }
        public async Task CheckForUpdates()
        {
            var process = true;
            var showProgress = false;

            var window = _checkConnectionFactory.Create();
            window.CancelToken += (_, _) => process = false;
            await Task.Delay(1000);
            var task = window.CheckGitHubConnection(window.TimerBlock, window.ProgressBar);
            var owner = _windowProvider.GetActiveWindow();
            await Task.Delay(1000);
            while (process)
            {
                if (task.IsCompleted) break;
                if (showProgress)
                    if (owner != null) await window.ShowDialog(owner);
                    else window.Show();
                await Task.Delay(100);
            }
            if (!await task) return;

            var updater = _updateManagerFactory.Create();
            if (await updater.CheckUpdateAsync(true)) return;
            if (updater.CheckVersion())
            {
                var title = _localizationService.GetMessageString("UpdatesAvailableTitle");
                var msg = _localizationService.GetMessageString("UpdatesAvailableText");
                var result = await _dialogService.QuestionDialogAsync(title, msg);
                if (result) await updater.UpdateAppAsync();
                return;
            }
            var titleNo = _localizationService.GetMessageString("ThisVersionLatestTitle");
            var msgNo = _localizationService.GetMessageString("ThisVersionLatestText");
            await _dialogService.ShowDefaultDialogAsync(titleNo, msgNo);
        }

        public AsyncRelayCommands ExecuteTasksCommand { get; }
        #endregion

        private async void UpdateCheckAsync()
        {
#if RELEASE
                    if (!_appSettings.AutoCheckUpdate
                        || _appSettings.LastUpdateCheck >= DateTime.Now.Date || !await _checkConnectionFactory.Create().CheckGitHubConnection()) return;
#endif

            var updater = _updateManagerFactory.Create();

            if (await updater.CheckUpdateAsync(false) && !updater.CheckVersion()) return;

            var titleYes = _localizationService.GetMessageString("UpdatesAvailableTitle");
            var msgYes = _localizationService.GetMessageString("UpdatesAvailableText");
            var result = await _dialogService.QuestionDialogAsync(titleYes, msgYes);
            if (result) await updater.UpdateAppAsync();
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

        public RelayCommands CheckBoxChanged { get; }
        public void CheckBoxChange(bool? isChecked, string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return;
            var settingsType = typeof(ProductProcessingOptions);
            var prop = settingsType.GetProperty(propertyName);
            TasksToRun.Add(propertyName);
            prop?.SetValue(null, isChecked);
            if (isChecked == true) ActiveExecutes++;
            else ActiveExecutes--;
        }

        public async Task ExecuteTasks()
        {
            try
            {
                if (!_taskFactory.HasCheckedCheckbox(ActiveExecutes))
                {
                    var message = _localizationService.GetString("MainWindow", "NoSetExecute_");
                    var title = _localizationService.GetString("MainWindow", "NoSetExecuteTitle_");
                    await _dialogService.ShowDefaultDialogAsync(title, message, MyIcon.Error);
                    return;
                }

                await ValidateDecimalTextBox();

                _fileManager.InitializeAllFiles();
                await _taskFactory.ExecuteOperations(TasksToRun);
                _fileManager.ClearAfterComplete();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                _logger.LogError(exception.Message);
            }
            finally
            {
                if (decimal.TryParse(IncreasePercentTextBox, out var increasePercentage))
                    ProductProcessingOptions.priceIncreasePercentage = increasePercentage;
            }
        }

        private async Task ValidateDecimalTextBox()
        {
            if (!ProductProcessingOptions.ShouldIncreasePrices) return;
            switch (ProductProcessingOptions.priceIncreasePercentage)
            {
                case < 100:
                    ProductProcessingOptions.priceIncreasePercentage = ProductProcessingOptions.priceIncreasePercentage / 100 + 1;
                    break;
                case >= 200:
                    {
                        var title = _localizationService.GetMessageString("Confirm");
                        var msg = _localizationService.GetMessageString("ConfirmText");
                        var formatedText = string.Format(msg, ProductProcessingOptions.priceIncreasePercentage.ToString(CultureInfo.InvariantCulture));

                        var result = await _dialogService.QuestionDialogAsync(title, formatedText, MyIcon.Question);
                        if (result) ProductProcessingOptions.priceIncreasePercentage = 1;
                        ProductProcessingOptions.priceIncreasePercentage /= 100;
                        break;
                    }
                default:
                    ProductProcessingOptions.priceIncreasePercentage /= 100;
                    break;
            }
        }


    }
}
