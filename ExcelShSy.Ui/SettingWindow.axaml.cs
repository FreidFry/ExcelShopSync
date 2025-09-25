using Avalonia;
using ExcelShSy.Core.Interfaces;
using ExcelShSy.Ui.Resources;
using ExcelShSy.Ui.Utils;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Ui
{
    public partial class SettingWindow : Window
    {
        private bool _isInitialized;

        #region Actions

        private bool _changeLanguage;
        private bool _dataBasePathChanged;

        #endregion
        
        private readonly string _currentLocalization;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILocalizationManager _localizationManager;
        private readonly IAppSettings _settings;
        
        public SettingWindow(IServiceProvider serviceProvider, ILocalizationManager localizationManager, IAppSettings settings)
        {
            _settings = settings;
            _currentLocalization = _settings.Language;
            _serviceProvider = serviceProvider;
            _localizationManager = localizationManager;
            InitializeControls();
            
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
                    Content = language.GetDescriptoin(),
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

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            _settings.SaveSettings(_settings);
            
            if (_changeLanguage && _settings.Language != _currentLocalization)
                ApplyLanguageChange();
            
            if(_dataBasePathChanged)
            {
            }
            
            this.Close();
        }

        private void CBLanguagues_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInitialized && sender is ComboBox { SelectedItem: ComboBoxItem { Tag: Enums.SupportedLanguagues selectedLang } })
            {
                var code = selectedLang.GetLangCode();
                _settings.Language = code;
                _changeLanguage = true;
            }
        }

        private void ShopManagerOpen_Click(object s, RoutedEventArgs e)
        {
            Window shopManagerWindow = _serviceProvider.GetRequiredService<ShopManagerWindow>();
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
            _dataBasePathChanged = true;
        }
        
        #endregion
        
        private void ApplyLanguageChange()
        {
            _localizationManager.SetCulture(_settings.Language);
            AvaloniaXamlLoader.Load(this);
            
            // Закрываем окно настроек
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var oldMainWindow = desktop.MainWindow;
                oldMainWindow.Hide();
                
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                desktop.MainWindow = mainWindow;
                mainWindow.Show();
                
                oldMainWindow.DataContext = null;
                oldMainWindow.Close();
                oldMainWindow = null;
            }
        }
    }
}
