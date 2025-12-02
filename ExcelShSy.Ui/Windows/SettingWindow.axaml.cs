using Avalonia.Controls;
using ExcelShSy.Core.Interfaces.ViewModels;

namespace ExcelShSy.Ui.Windows
{
    public partial class SettingWindow : Window
    {
        public SettingWindow(ISettingViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }
    }
}
