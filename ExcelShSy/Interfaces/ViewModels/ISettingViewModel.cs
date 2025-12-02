using Avalonia.Platform.Storage;

namespace ExcelShSy.Core.Interfaces.ViewModels
{
    public interface ISettingViewModel
    {
        void SetStorageProvider(IStorageProvider storageProvider);
        Task SelectDataBasePathAsync();
        Task OpenShopManagerAsync();
        Task ApplyAsync();
    }
}
