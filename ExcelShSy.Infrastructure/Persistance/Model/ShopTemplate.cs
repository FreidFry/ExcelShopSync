using ExcelShSy.Core.Interfaces.Shop;

namespace ExcelShSy.Infrastructure.Persistance.Model
{
    public class ShopTemplate : IShopTemplate
    {

        public string Name { get; set; }
        public List<string> UnmappedHeaders { get; set; }

        public IReadOnlyDictionary<string, string> AvailabilityMap { get; set; }

        public string DataFormat { get; set; }

        public string? Article { get; set; }

        public string? Price { get; set; }

        public string? OldPrice { get; set; }

        public string? Availability { get; set; }

        public string? Quantity { get; set; }

        public string? Discount { get; set; }

        public string? DiscountDateStart { get; set; }

        public string? DiscountDateEnd { get; set; }

        public IShopTemplate Clone()
        {
            return new ShopTemplate
            {
                Name = this.Name,
                UnmappedHeaders = new List<string>(this.UnmappedHeaders),
                AvailabilityMap = new Dictionary<string, string>(this.AvailabilityMap),
                DataFormat = this.DataFormat,
                Article = this.Article,
                Price = this.Price,
                OldPrice = this.OldPrice,
                Availability = this.Availability,
                Quantity = this.Quantity,
                Discount = this.Discount,
                DiscountDateStart = this.DiscountDateStart,
                DiscountDateEnd = this.DiscountDateEnd
            };
        }
    }
}
