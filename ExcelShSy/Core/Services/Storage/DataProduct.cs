using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Core.Services.Storage
{
    public class DataProduct : IDataProduct
    {
        public Dictionary<string, decimal> Price { get; set; } = [];
        public Dictionary<string, decimal> Quantity { get; set; } = [];
        public Dictionary<string, string> Availability { get; set; } = [];
        public Dictionary<string, decimal> Discount {  get; set; } = [];
        public Dictionary<string, DateOnly> DiscountFrom { get; set; } = [];
        public Dictionary<string, DateOnly> DiscountTo { get; set; } = [];

        public void AddProductPrice(string productName, decimal price)
        {
            if (Price.ContainsKey(productName))
                return;
            Price.Add(productName, price);
        }

        public void AddProductAvailability(string productName, string availability)
        {
            if (Availability.ContainsKey(productName))
                return;
            Availability.Add(productName, availability);
        }

        public void AddProductQuantity(string productName, decimal quantity)
        {
            if (Quantity.ContainsKey(productName))
                return;
            Quantity.Add(productName, quantity);
        }

        public void AddProductDiscount(string productName, decimal discount)
        {
            if (Discount.ContainsKey(productName))
                return;
            Discount.Add(productName, discount);
        }

        public void ClearAll()
        {
            Price.Clear();
            Quantity.Clear();
            Availability.Clear();
            Discount.Clear();
        }

        public void AddProductDiscountFrom(string productName, DateOnly discount)
        {
            if (DiscountFrom.ContainsKey(productName))
                return;
            DiscountFrom.Add(productName, discount);
        }

        public void AddProductDiscountTo(string productName, DateOnly discount)
        {
            if (DiscountTo.ContainsKey(productName))
                return;
            DiscountTo.Add(productName, discount);
        }
    }
}
