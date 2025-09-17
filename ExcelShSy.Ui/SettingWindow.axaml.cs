using System.Text.Json;
using Avalonia;
using ExcelShSy.Core.Interfaces;
using ExcelShSy.Localization.Properties;
using ExcelShSy.Ui.Resources;
using ExcelShSy.Ui.Utils;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Ui
{
    public partial class SettingWindow : Window
    {
        private readonly string _currentLocalization;
        private string _newLocalization;
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
        }

        private void CBLanguagues_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox combo && combo.SelectedItem is ComboBoxItem item && item.Tag is Enums.SupportedLanguagues selectedLang)
            {
                var code = selectedLang.GetLangCode();
                _newLocalization = code;
                _changeLanguage = true;
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (_changeLanguage && _newLocalization != _currentLocalization)
                ApplyLanguageChange();
            this.Close();
        }

        private void InitializeControls()
        {
            InitializeComponent();

            foreach (var language in Helpers.GetEnums<Enums.SupportedLanguagues>())
            {
                var cbi = new ComboBoxItem()
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

        private async void ApplyLanguageChange()
        {
            _localizationManager.SetCulture(_newLocalization);
            _localizationManager.SaveSettings(new AppSettings { Language = _newLocalization });

            // Закрываем окно настроек
            this.Close();

            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                desktop.MainWindow.Close();
                desktop.MainWindow = mainWindow;
                mainWindow.Show();
            }
        }

        private void ShopManagerOpen_Click(object s, RoutedEventArgs e)
        {
            Window shopManagerWindow = _serviceProvider.GetRequiredService<ShopManagerWindow>();
            shopManagerWindow.ShowDialog(this);
        }
    }
}
