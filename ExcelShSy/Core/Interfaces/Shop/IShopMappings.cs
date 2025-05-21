namespace ExcelShSy.Core.Interfaces.Shop
{
    public interface IShopMappings
    {
        Dictionary<string, IShopTemplate> Shops { get; }
    }
}