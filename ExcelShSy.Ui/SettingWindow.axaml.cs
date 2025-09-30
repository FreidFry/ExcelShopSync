using System.ComponentModel;
using ExcelShSy.Core.Interfaces;
using ExcelShSy.Ui.Resources;
using ExcelShSy.Ui.Utils;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ExcelShSy.Core.Interfaces.Common;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace ExcelShSy.Ui
{
    public partial class SettingWindow : Window, INotifyPropertyChanged
    {
        #region State
        
        private readonly bool _isInitialized;

        #endregion
        
        #region Actions

        private bool _shouldLangChanged;
        private bool _shouldMoveDatabase;

        #endregion

        #region Dipedency Injection

        private readonly string _currentLocalization;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILocalizationManager _localizationManager;
        private readonly IAppSettings _settings;

        #endregion

        public SettingWindow()
        {
            InitializeComponent();
        }
        
        public SettingWindow(IServiceProvider serviceProvider, ILocalizationManager localizationManager, IAppSettings settings)
        {
            _settings = settings;
            _currentLocalization = _settings.Language;
            _serviceProvider = serviceProvider;
            _localizationManager = localizationManager;
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
                    Content = Helpers.GetDescription(language),
                    Tag = language,
                };
                CBLanguagues.Items.Add(cbi);
                if (language.GetLangCode() == _settings.Language)
                {
                    CBLanguagues.SelectedItem = cbi;
                }
            }
        }
        
        private void InitializeDbPath() =>
            DataBasePath.Text = _settings.DataBasePath;

        #endregion

        #region Actions
        
        #region Clicks
        
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            _settings.SaveSettings(_settings);
            
            if (_shouldLangChanged && _settings.Language != _currentLocalization)
                _localizationManager.SetCulture(_settings.Language);
            
            if(_shouldMoveDatabase)
            {
            }
            
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
            _settings.DataBasePath = selectedFolderPath;
            _shouldMoveDatabase = true;
        }
        
        #endregion
        
        public bool CreateNewFileWhileSave
        {
            get => _settings.CreateNewFileWhileSave;
            set
            {
                if (_settings.CreateNewFileWhileSave != value)
                {
                    _settings.CreateNewFileWhileSave = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreateNewFileWhileSave)));
                }
            }
        }

        private void CBLanguagues_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInitialized && sender is ComboBox { SelectedItem: ComboBoxItem { Tag: Enums.SupportedLanguagues selectedLang } })
            {
                var code = selectedLang.GetLangCode();
                _settings.Language = code;
                _shouldLangChanged = true;
            }
        }
        
        #endregion

        #region Events

        public new event PropertyChangedEventHandler? PropertyChanged;

        #endregion
    }
}
