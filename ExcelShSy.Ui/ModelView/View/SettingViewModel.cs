using Avalonia.Platform.Storage;
using ExcelShSy.Core.Enums;
using ExcelShSy.Core.Helpers;
using ExcelShSy.Core.Interfaces;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.ViewModels;
using ExcelShSy.Ui.Interfaces;
using ExcelShSy.Ui.ModelView.Base;
using ExcelShSy.Ui.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace ExcelShSy.Ui.ModelView.View
{
    public class SettingViewModel : ViewModelBase, ISettingViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILocalizationManager _localizationManager;
        private readonly IAppSettings _settings;
        private readonly IAppSettings _newSettings;
        private readonly ILogger _logger;
        private IStorageProvider? _storageProvider;
        private readonly IWindowProvider _windowProvider;

        public SettingViewModel(IServiceProvider serviceProvider, ILocalizationManager localizationManager, IAppSettings settings, ILogger logger, IWindowProvider windowProvider, IStorageProvider? storageProvider = null)
        {
            _settings = settings;
            _serviceProvider = serviceProvider;
            _localizationManager = localizationManager;
            _windowProvider = windowProvider;
            _logger = logger;
            _storageProvider = storageProvider;

            _newSettings = serviceProvider.GetRequiredService<IConfigurationManager>().Load();

            #region Initialize Fields

            _createNewFileWhileSave = _newSettings.CreateNewFileWhileSave;
            _autoCheckUpdate = _newSettings.AutoCheckUpdate;
            _languages = EnumHelper.GetEnums<LanguaguesEnum.SupportedLanguagues>();
            _selectedLanguage = _newSettings.Language;
            _dataBasePath = _newSettings.DataBasePath;

            #endregion

            #region Initialize Commands

            SelectDataBasePathCommand = new AsyncRelayCommands(async _ => await SelectDataBasePathAsync());
            OpenShopManagerCommand = new AsyncRelayCommands(async _ => await OpenShopManagerAsync());
            ApplyCommand = new AsyncRelayCommands(async _ => await ApplyAsync());

            #endregion
        }

        public void SetStorageProvider(IStorageProvider storageProvider)
        {
            _storageProvider ??= storageProvider;
        }

        #region Fields

        private bool _createNewFileWhileSave;
        private bool _autoCheckUpdate;
        private readonly LanguaguesEnum.SupportedLanguagues[] _languages;
        private LanguaguesEnum.SupportedLanguagues _selectedLanguage;
        private string _dataBasePath;

        #endregion

        #region Properties

        public bool CreateNewFileWhileSave
        {
            get => _createNewFileWhileSave;
            set
            {
                if (SetProperty(ref _createNewFileWhileSave, value))
                    _newSettings.CreateNewFileWhileSave = value;
            }
        }

        public bool AutoCheckUpdate
        {
            get => _autoCheckUpdate;
            set
            {
                if (SetProperty(ref _autoCheckUpdate, value))
                    _newSettings.AutoCheckUpdate = value;
            }
        }

        public LanguaguesEnum.SupportedLanguagues[] Languages
        {
            get => _languages;
        }

        public LanguaguesEnum.SupportedLanguagues SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (SetProperty(ref _selectedLanguage, value) && _settings.Language != value)
                {
                    _newSettings.Language = value;
                    _shouldLangChanged = true;
                };
            }
        }

        public string DataBasePath
        {
            get => _dataBasePath;
            set
            {
                if (SetProperty(ref _dataBasePath, value))
                    _newSettings.DataBasePath = value;
            }
        }

        #endregion

        #region Relay Commands

        public AsyncRelayCommands SelectDataBasePathCommand { get; }

        public async Task SelectDataBasePathAsync()
        {
            if (_storageProvider == null)
            {
                _logger.LogError("StorageProvider is not set in SettingViewModel.");
                return;
            }

            var options = new FolderPickerOpenOptions
            {
                Title = "Select a Folder",
                AllowMultiple = false
            };

            var folders = await _storageProvider.OpenFolderPickerAsync(options);

            if (folders.Count <= 0) return;

            var selectedFolderPath = folders[0].Path.LocalPath;
            DataBasePath = selectedFolderPath;
            _newSettings.DataBasePath = selectedFolderPath;
        }

        public AsyncRelayCommands OpenShopManagerCommand { get; }

        public async Task OpenShopManagerAsync()
        {
            var shopManagerWindow = _serviceProvider.GetRequiredService<ShopManagerWindow>();
            var windowProvider = _windowProvider.GetActiveWindow();
            if (windowProvider != null)
                await shopManagerWindow.ShowDialog(windowProvider);
            else
                shopManagerWindow.Show();
        }

        public AsyncRelayCommands ApplyCommand { get; }

        public async Task ApplyAsync()
        {
            if (_shouldLangChanged && _settings.Language != _newSettings.Language)
                _localizationManager.SetCulture(_newSettings.LanguageCode);

            _settings.SaveSettings(_newSettings);
        }

        #endregion

        #region Flags

        private bool _shouldLangChanged;

        #endregion
    }
}
