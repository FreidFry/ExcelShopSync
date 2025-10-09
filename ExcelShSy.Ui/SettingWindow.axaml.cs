using System.ComponentModel;
using ExcelShSy.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Localization;
using ExcelShSy.Localization.Resources;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace ExcelShSy.Ui
{
    public partial class SettingWindow : Window, INotifyPropertyChanged
    {
        #region State
        
        private readonly bool _isInitialized;

        #endregion
        
        #region Fields
        
        public bool CreateNewFileWhileSave
        {
            get => _newSettings.CreateNewFileWhileSave;
            set
            {
                if (_newSettings.CreateNewFileWhileSave != value)
                {
                    _newSettings.CreateNewFileWhileSave = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreateNewFileWhileSave)));
                }
            }
        }
        
        public bool AutoCheckUpdate
        {
            get => _newSettings.CheckForUpdates;
            set
            {
                if (_newSettings.CheckForUpdates != value)
                {
                    _newSettings.CheckForUpdates = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AutoCheckUpdate)));
                }
            }
        }
        
        #endregion
        
        #region Actions

        private bool _shouldLangChanged;
        private bool _shouldMoveDatabase;

        #endregion

        #region Dipedency Injection

        private readonly IServiceProvider _serviceProvider;
        private readonly ILocalizationManager _localizationManager;
        private readonly IAppSettings _settings;
        private readonly IAppSettings _newSettings;

        #endregion

#if DESIGNER
        public SettingWindow()
        {
            InitializeComponent();
        }
#endif
        
        public SettingWindow(IServiceProvider serviceProvider, ILocalizationManager localizationManager, IAppSettings settings)
        {
            _settings = settings;
            _serviceProvider = serviceProvider;
            _localizationManager = localizationManager;
            
            _newSettings = serviceProvider.GetRequiredService<IConfigurationManager>().Load();
            
            InitializeControls();
            DataContext = this;
            _isInitialized = true;
        }

        #region Start initoalization

        private void InitializeControls()
        {
            InitializeComponent();
            
            InitialLanguages();
            InitializeDbPath();
        }
        
        private void InitialLanguages()
        {
            foreach (var language in Helpers.GetEnums<Enums.SupportedLanguagues>())
            {
                var cbi = new ComboBoxItem
                {
                    Content = language.GetDescription(),
                    Tag = language,
                };
                CBLanguagues.Items.Add(cbi);
                if (language.GetLangCode() == _newSettings.Language)
                {
                    CBLanguagues.SelectedItem = cbi;
                }
            }
        }
        
        private void InitializeDbPath() =>
            DataBasePath.Text = _newSettings.DataBasePath;

        #endregion

        #region Actions
        
        #region Clicks
        
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (_shouldLangChanged && _settings.Language != _newSettings.Language)
                _localizationManager.SetCulture(_newSettings.Language);
            
            _settings.SaveSettings(_newSettings);
            
            Close();
        }
        
        private void ShopManagerOpen_Click(object s, RoutedEventArgs e)
        {
            var shopManagerWindow = _serviceProvider.GetRequiredService<ShopManagerWindow>();
            shopManagerWindow.ShowDialog(this);
        }
        
        private async void SelectDataBasePath_Click(object? sender, RoutedEventArgs e)
        {
            var storageProvider = StorageProvider;
            var options = new FolderPickerOpenOptions
            {
                Title = "Select a Folder",
                AllowMultiple = false
            };

            var folders = await storageProvider.OpenFolderPickerAsync(options);

            if (folders.Count <= 0) return;
            
            var selectedFolderPath = folders[0].Path.LocalPath;
            DataBasePath.Text = selectedFolderPath;
            _newSettings.DataBasePath = selectedFolderPath;
            _shouldMoveDatabase = true;
        }
        
        #endregion

        private void CBLanguagues_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInitialized && sender is ComboBox { SelectedItem: ComboBoxItem { Tag: Enums.SupportedLanguagues selectedLang } })
            {
                var code = selectedLang.GetLangCode();
                _newSettings.Language = code;
                _shouldLangChanged = true;
            }
        }
        
        #endregion

        #region Events

        public new event PropertyChangedEventHandler? PropertyChanged;

        #endregion
    }
}
