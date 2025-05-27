namespace ExcelShSy.Core.Interfaces.Shop
{
    public interface IShopMappings
    {
        IShopTemplate GetShop(string shopName);
        Dictionary<string, IShopTemplate> Shops { get; }
    }
}