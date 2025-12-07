using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using ExcelShSy.Core.Interfaces.ViewModels;
using ExcelShSy.Ui.Interfaces;
using System.Diagnostics;

namespace ExcelShSy.Ui.Windows
{
    public partial class ShopManagerWindow : Window
    {
#if DESIGNER
        public ShopManagerWindow()
        {
            InitializeComponent();
        }
#endif

        public ShopManagerWindow(IShopManagerViewModel vm, IWindowProvider windowProvider)
        {
            InitializeComponent();
            DataContext = vm;
            Closing += OnClosing;
            
            windowProvider.RegisterWindow(this);
            Closing += (_, _) => windowProvider.UnregisterWindow(this);
        }

        #region Initialization

        private async void OnClosing(object? sender, WindowClosingEventArgs e)
        {
            if (DataContext is IShopManagerViewModel vm)
            {
                var canClose = await vm.TryCloseAsync();
                if (!canClose)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        #endregion

        private void ShowDataFormatTooltip(object? sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo()
            {
                FileName = SelectGuidePage(),
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private static string SelectGuidePage()
        {
            var language = Thread.CurrentThread.CurrentCulture.Name;
            const string fileName = "Formats";
            var fileDirectory = Path.Combine(Environment.CurrentDirectory, "Web", "DataFormats");
            var path = Path.Combine(fileDirectory, $"{fileName}.{language}.html");
            var baseFile = Path.Combine(fileDirectory, $"{fileName}.html");

            return File.Exists(path) ? path : baseFile;
        }

        private void IsChanged_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is IShopManagerViewModel vm)
            {
                if (vm.IsInitialized)
                vm.ShopChanged = true;
            }
        }

        private void IsChanged_Changed(object sender, TextChangedEventArgs e)
        {
            if (DataContext is IShopManagerViewModel vm)
            {
                if (vm.IsInitialized)
                    vm.ShopChanged = true;
            }
        }
    }
}