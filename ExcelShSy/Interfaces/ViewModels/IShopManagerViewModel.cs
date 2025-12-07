using Avalonia.Interactivity;
using System.Collections.ObjectModel;

namespace ExcelShSy.Core.Interfaces.ViewModels
{
    public interface IShopManagerViewModel
    {
        ObservableCollection<string> AllShopList { get; }
        ObservableCollection<string?> ShopHeaders { get; }
        bool IsInitialized { get; set; }
        bool ShopChanged { get; set; }

        Task CreateShopAsync();
        Task DropShopAsync();
        Task GetHeadersWithFileAsync();
        Task LoadShopAsync(string shopName);
        void OnChanged(object sender, RoutedEventArgs e);
        void OnLoaded();
        Task RemoveSelectedHeadersAsync(IList<string> needRemove);
        Task RenameShopAsync();
        void SaveShop();
        Task<bool> TryCloseAsync();
    }
}
