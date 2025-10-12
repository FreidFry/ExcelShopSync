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
using ExcelShSy.Ui.Utils;

namespace ExcelShSy.Ui
{
    public partial class MainWindow : Window
    {
        private readonly IAppSettings _appSettings;
        private readonly IFileManager _fileManager;
        private readonly IOperationTaskFactory _taskFactory;
        private readonly IEditLoadFilesWindowFactory _editLoadFilesWindowFactory;
        private readonly ISettingWindowFactory _settingWindowFactory;
        private readonly IDataBaseViewerFactory _dataBaseViewer;
        private readonly IF4LabsAboutWindowFactory _f4LabsAboutWindowFactory;
        private readonly ILocalizationService _localizationService;

        #if DESIGNER
        public MainWindow()
        {
            InitializeComponent();
        }
        #endif
        
        public MainWindow(IAppSettings appSettings, IFileManager fileManager, IOperationTaskFactory taskFactory, IEditLoadFilesWindowFactory editLoadFilesWindowFactory, ISettingWindowFactory settingWindowFactory, IDataBaseViewerFactory dataBaseViewer,
           IF4LabsAboutWindowFactory f4LabsAboutWindowFactory,ILocalizationService localizationService)
        {
            InitializeComponent();

            _appSettings = appSettings;
            _fileManager = fileManager;
            _taskFactory = taskFactory;
            _editLoadFilesWindowFactory = editLoadFilesWindowFactory;
            _settingWindowFactory = settingWindowFactory;
            _dataBaseViewer = dataBaseViewer;
            _f4LabsAboutWindowFactory = f4LabsAboutWindowFactory;
            _localizationService = localizationService;

            UpdateTextBlockEvents.RegistrationTextBlockEvent("TargetLb", TargetLastFile);
            UpdateTextBlockEvents.RegistrationTextBlockEvent("SourceLb", SourceLastFile);

            Loaded += UpdateCheck;
        }

        private async void UpdateCheck(object? sender, RoutedEventArgs e)
        {
            if (_appSettings.CheckForUpdates 
                && _appSettings.LastUpdateCheck < DateTime.Now.Date
               )
            {
                var updater = new UpdateManager(_localizationService, _appSettings);
                var isUpdateAvailable = await updater.Check();
                if (!isUpdateAvailable)
                    return;

                var titleYes = _localizationService.GetMessageString("UpdatesAvailableTitle");
                var msgYes = _localizationService.GetMessageString("UpdatesAvailableText");
                var resultYes = await MessageBoxManager
                    .GetMessageBoxStandard(titleYes, msgYes, ButtonEnum.YesNo)
                    .ShowWindowAsync();
                if (resultYes == ButtonResult.Yes) updater.UpdateApp();
            }
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

        private async void ExecuteTasks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_taskFactory.HasCheckedCheckbox(TasksContainer))
                {
                    var message = _localizationService.GetString("MainWindow", "NoSetExecute_");
                    var title = _localizationService.GetString("MainWindow", "NoSetExecuteTitle_");
                    var msBox = MessageBoxManager.GetMessageBoxStandard(title, message, ButtonEnum.Ok,
                        MsBox.Avalonia.Enums.Icon.Error);
                    await msBox.ShowWindowDialogAsync(this);
                    return;
                }

                await ValidateDecimalTextBox();

                _fileManager.InitializeAllFiles();
                _taskFactory.ExecuteOperations(TasksContainer);
                _fileManager.ClearAfterComplete();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
            finally
            {
                if(decimal.TryParse(IncreasePercentTextBox.Text, out var increasePercentage))
                    ProductProcessingOptions.priceIncreasePercentage = increasePercentage;
            }
            
        }
        
        private async Task ValidateDecimalTextBox()
        {
            if (!ProductProcessingOptions.ShouldIncreasePrices) return;
            switch (ProductProcessingOptions.priceIncreasePercentage)
            {
                case < 100:
                    ProductProcessingOptions.priceIncreasePercentage /= 10;
                    break;
                case >= 200:
                {
                    var title = _localizationService.GetMessageString("Confirm");
                    var msg = _localizationService.GetMessageString("ConfirmText");
                    var formatedText = string.Format(msg, ProductProcessingOptions.priceIncreasePercentage.ToString(CultureInfo.InvariantCulture));
                    
                    var msBox = MessageBoxManager.GetMessageBoxStandard(title,
                        formatedText,
                        ButtonEnum.YesNo);
                    var result = await msBox.ShowWindowDialogAsync(this);
                    if (result != ButtonResult.Yes) ProductProcessingOptions.priceIncreasePercentage = 1;
                    break;
                }
                default:
                    ProductProcessingOptions.priceIncreasePercentage /= 100;
                    break;
            }
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

        private async void CheckForUpdates_Click(object? sender, RoutedEventArgs e)
        {
            var updater = new UpdateManager(_localizationService, _appSettings);
            if (await updater.Check())
            {
                var title = _localizationService.GetMessageString("UpdatesAvailableTitle");
                var msg = _localizationService.GetMessageString("UpdatesAvailableText"); 
                var result = await MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.YesNo).ShowAsync();
                if (result == ButtonResult.Yes)
                    updater.UpdateApp();
                return;
            }
            var titleNo = _localizationService.GetMessageString("ThisVersionLatestTitle");
            var msgNo = _localizationService.GetMessageString("ThisVersionLatestText");
            await MessageBoxManager.GetMessageBoxStandard(titleNo, msgNo).ShowWindowAsync();
        }
    }
}
