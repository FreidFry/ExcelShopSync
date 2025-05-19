namespace ExcelShSy.Core.Interfaces
{
    public interface IShopMappings
    {
        Dictionary<string, IShopTemplate> Shops { get; }
    }
}