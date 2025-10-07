namespace ExcelShSy.Core.Interfaces.Shop
{
    public interface IShopTemplate
    {
        string Name { get; set; }
        List<string> UnmappedHeaders { get; set; }
        Dictionary<string, string?> AvailabilityMap { get; set; }
        string? DataFormat { get; set; }

        string? Article { get; set; }
        string? Price { get; set; }
        string? Availability { get; set; }
        string? Quantity { get; set; }
        string? Discount { get; set; }
        string? DiscountDateStart { get; set; }
        string? DiscountDateEnd { get; set; }
        string? OldPrice { get; set; }

        public IShopTemplate Clone();
    }
}
