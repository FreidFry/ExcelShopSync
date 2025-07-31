using ExcelShSy.Core.Interfaces.Common;

using System.Windows;
using System.Windows.Controls;

namespace ExcelShSy.Ui
{
    public partial class SettingWindow : Window
    {
        private readonly ILocalizationService _localizationService;

        public SettingWindow(ILocalizationService localizationService)
        {
            InitializeComponent();
            _localizationService = localizationService;
        }

        private void LangSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox combo && combo.SelectedItem is ComboBoxItem item)
            {
                var cultureCode = (string)item.Tag;
                _localizationService.SetCulture(cultureCode);
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
