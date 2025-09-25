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
    }
}