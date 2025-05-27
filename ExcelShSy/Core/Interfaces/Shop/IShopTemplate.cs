namespace ExcelShSy.Core.Interfaces.Shop
{
    public interface IShopTemplate
    {
        IReadOnlyList<string> Columns { get; }
        IReadOnlyDictionary<string, string> Availability { get; }
    }
}
