namespace ExcelShSy.Core.Interfaces.Shop
{
    public interface IShopMapping
    {
        IShopTemplate FindShopTemplate(string shopName);
        Dictionary<string, IShopTemplate> Shops { get; }
    }
}