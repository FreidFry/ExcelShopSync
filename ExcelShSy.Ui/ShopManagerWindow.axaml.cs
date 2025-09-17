using System.Collections.ObjectModel;
using ExcelShSy.Core.Interfaces.Shop;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Ui
{
    public partial class ShopManagerWindow : Window, INotifyPropertyChanged
    {
        private readonly IShopStorage _shopStorage;

        private bool _shopChanged = false;
        private bool _ready = false;

        #region Current Shop

        private IShopTemplate _currentShop;
        public IShopTemplate CurrentShop
        {
            get => _currentShop;
            set
            {
                if (_currentShop != value)
                {
                    _currentShop = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Shop Headers
        private ObservableCollection<string> _currentShopHeaders = [];
        public ObservableCollection<string> CurrentShopHeaders
        {
            get => _currentShopHeaders;
            set
            {
                if (_currentShopHeaders != value)
                {
                    _currentShopHeaders = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        public new event PropertyChangedEventHandler? PropertyChanged;

        public ShopManagerWindow(IShopStorage shopStorage)
        {
            _shopStorage = shopStorage;
            InitializeComponent();
            DataContext = this;

            Loaded += (s, e) =>
            {
                LoadMagazineInSelector();
                _ready = true;
            };
        }

        private void LoadMagazineInSelector()
        {
            var ShopList = _shopStorage.GetShopList();
            if (ShopList.Count == 0)
            {
                return;
            }

            MagazineSelector.ItemsSource = ShopList;
            MagazineSelector.SelectedIndex = 0;
        }

        private async void SelectMagazine_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (_shopChanged)
            {
                var msBox = MessageBoxManager.GetMessageBoxStandard("Unsaved Changes", "You have unsaved changes. Do you want to save them?", ButtonEnum.YesNoCancel);
                var result = await msBox.ShowAsync();
                if (result == ButtonResult.Yes)
                {
                    SaveShop();
                }
                else if (result == ButtonResult.Cancel)
                    return;
            }
            if (MagazineSelector.SelectedItem is string selectedShop)
            {
                var shop = _shopStorage.GetShopMapping(selectedShop);
                foreach (var header in shop.UnmappedHeaders)
                    CurrentShopHeaders.Add(header);
                CurrentShop = shop;
                _shopChanged = false;
            }
        }


        private async void SaveShopTemplate_Click(object sender, RoutedEventArgs e)
        {
            var msBox = MessageBoxManager.GetMessageBoxStandard( "Confirm Save","Are you sure you want to save the changes?", ButtonEnum.YesNo);
            var result = await msBox.ShowAsync();
            if (result != ButtonResult.Yes)
                return;
            SaveShop();
        }

        private void SaveShop()
        {
            _shopStorage.UpdateShop(CurrentShop);
            _shopStorage.SaveShopTemplate(CurrentShop);
            _shopChanged = false;
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void IsChanged_Changed(object sender, RoutedEventArgs e)
        {
            if (_ready)
                _shopChanged = true;
        }
    }
}