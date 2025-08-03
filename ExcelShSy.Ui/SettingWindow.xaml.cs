using ExcelShSy.Core.Interfaces;
using ExcelShSy.Ui.Properties;
using ExcelShSy.Ui.Resources;
using ExcelShSy.Ui.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace ExcelShSy.Ui
{
    public partial class SettingWindow : Window
    {
        private readonly string _currentLocalization;
        private string _newLocalization;
        private bool _changeLanguage = false;

        private readonly IServiceProvider _serviceProvider;
        private readonly ILocalizationManager _localizationManager;
        public SettingWindow(IServiceProvider serviceProvider, ILocalizationManager localizationManager)
        {
            _currentLocalization = Settings.Default.Language;
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
                if (language.GetLangCode() == Settings.Default.Language)
                {
                    CBLanguagues.SelectedItem = cbi;
                }
            }

        }

        private void ApplyLanguageChange()
        {
            _localizationManager.SetCulture(_newLocalization);

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }
    }
}
