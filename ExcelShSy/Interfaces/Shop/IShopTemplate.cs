namespace ExcelShSy.Core.Interfaces.Shop
{
    public interface IShopTemplate
    {
        IReadOnlyList<string> UnmappedHeaders { get; }
        IReadOnlyDictionary<string, string> AvailabilityMap { get; }
        string DataFormat { get; }
    }
}
