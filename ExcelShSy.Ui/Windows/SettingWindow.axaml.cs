using Avalonia.Controls;
using ExcelShSy.Core.Interfaces.ViewModels;
using ExcelShSy.Ui.Interfaces;

namespace ExcelShSy.Ui.Windows
{
    public partial class SettingWindow : Window
    {
        public SettingWindow(ISettingViewModel vm, IWindowProvider windowProvider)
        {
            InitializeComponent();

            DataContext = vm;

            windowProvider.RegisterWindow(this);
            Closing += (_, _) => windowProvider.UnregisterWindow(this);
        }
    }
}
