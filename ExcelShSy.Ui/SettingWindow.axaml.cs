using Avalonia;
using ExcelShSy.Core.Interfaces;
using ExcelShSy.Ui.Resources;
using ExcelShSy.Ui.Utils;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Ui
{
    public partial class SettingWindow : Window
    {
        private bool _isInitialized = false;
        
        private readonly string _currentLocalization;
        private bool _changeLanguage = false;
        private readonly IAppSettings _settings;

        private readonly IServiceProvider _serviceProvider;
        private readonly ILocalizationManager _localizationManager;
        public SettingWindow(IServiceProvider serviceProvider, ILocalizationManager localizationManager, IAppSettings settings)
        {
            _settings = settings;
            _currentLocalization = _settings.Language;
            _serviceProvider = serviceProvider;
            _localizationManager = localizationManager;
            InitializeControls();
            
            _isInitialized = true;
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

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            _settings.SaveSettings(_settings);
            
            if (_changeLanguage && _settings.Language != _currentLocalization)
                ApplyLanguageChange();
            this.Close();
        }

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

        private async void ApplyLanguageChange()
        {
            _localizationManager.SetCulture(_settings.Language);

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
        }
    }
}
