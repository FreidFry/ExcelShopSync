using ExcelShSy.Core.Interfaces.Shop;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace ExcelShSy.Ui
{
    public partial class ShopManagerWindow : Window, INotifyPropertyChanged
    {
        private readonly IShopStorage _shopStorage;

        private bool _ShopChanged = false;
        private bool Ready = false;

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
        private List<string> _currentShopHeaders;
        public List<string> CurrentShopHeaders
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public ShopManagerWindow(IShopStorage shopStorage)
        {
            _shopStorage = shopStorage;
            InitializeComponent();
            DataContext = this;

            Loaded += (s, e) =>
            {
                LoadMagazineInSelector();
                Ready = true;
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

        private void SelectMagazine_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (_ShopChanged)
            {
                var question = MessageBox.Show("You have unsaved changes. Do you want to save them?", "Unsaved Changes", MessageBoxButton.YesNoCancel);
                if (question == MessageBoxResult.Yes)
                {
                    SaveShop();
                }
                else if (question == MessageBoxResult.Cancel)
                    return;
            }
            if (MagazineSelector.SelectedItem is string selectedShop)
            {
                var shop = _shopStorage.GetShopMapping(selectedShop);
                CurrentShop = shop;
                CurrentShopHeaders = CurrentShop.UnmappedHeaders;
                _ShopChanged = false;
            }
        }


        private void SaveShopTemplate_Click(object sender, RoutedEventArgs e)
        {
            var question = MessageBox.Show("Are you sure you want to save the changes?", "Confirm Save", MessageBoxButton.YesNo);
            if (question != MessageBoxResult.Yes)
                return;
            SaveShop();
        }

        private void SaveShop()
        {
            _shopStorage.UpdateShop(CurrentShop);
            _shopStorage.SaveShopTemplate(CurrentShop);
            _ShopChanged = false;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void IsChanged_Changed(object sender, RoutedEventArgs e)
        {
            if (Ready)
                _ShopChanged = true;
        }
    }
}