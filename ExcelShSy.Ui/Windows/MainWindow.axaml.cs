using Avalonia.Controls;
using ExcelShSy.Core.Interfaces.ViewModels;
using ExcelShSy.Core.Properties;
using ExcelShSy.Event;
using ExcelShSy.Ui.Interfaces;
using System.Globalization;

namespace ExcelShSy.Ui.Windows
{
    public partial class MainWindow : Window
    {
#if DESIGNER
        public MainWindow()
        {
            InitializeComponent();
        }
#endif
        public MainWindow(IMainViewModel vm, IWindowProvider windowProvider)
        {
            InitializeComponent();
            DataContext = vm;

            UpdateTextBlockEvents.RegistrationTextBlockEvent("TargetLb", TargetLastFile);
            UpdateTextBlockEvents.RegistrationTextBlockEvent("SourceLb", SourceLastFile);

            //Loaded += UpdateCheckAsync;
        }

        private void ChangeIncreasePercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox textBox )
                return;
            if(textBox.Text == null)
                return;

            if (DataContext is not IMainViewModel vm)
                return;

            var filtered = IsTextAllowed(textBox.Text);

            if (filtered != vm.IncreasePercentTextBox)
                vm.IncreasePercentTextBox = filtered;
            textBox.Text = filtered;
            textBox.CaretIndex = filtered.Length;

        }

        private static string IsTextAllowed(string newInput) =>
    new([.. newInput.Where(c => char.IsDigit(c) || c == '.' || c == ',')]);


        //private void CheckBox_Click(object sender, RoutedEventArgs e)
        //{
        //    var cb = sender as CheckBox;
        //    var propertyName = cb?.Tag?.ToString();
        //    var value = cb?.IsChecked == true;

        //    if (!string.IsNullOrEmpty(propertyName))
        //    {
        //        var settingsType = typeof(ProductProcessingOptions);
        //        var prop = settingsType.GetProperty(propertyName);
        //        prop?.SetValue(null, value);
        //    }
        //}
    }
}