namespace ExcelShSy.Core.Interfaces.Shop
{
    public interface IShopStorage
    {
        IShopTemplate? GetShopMapping(string shopName);
        List<string> GetShopList();
        void SaveShopTemplate(IShopTemplate shop);
        void UpdateShop(IShopTemplate updatedShop);
        List<IShopTemplate> Shops { get; set; }
        void AddShop(string shopName);
        event Action<string>? ShopsChanged;
        void RemoveShop(string shopName);
        void RenameShop(string oldName, string newName);
        bool IsFileNotExist(string path);
    }
}