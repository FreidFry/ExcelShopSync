using ExcelShSy.Core.Interfaces.Common;

namespace ExcelShSy.Infrastructure.Services.Storage
{
    public class DataProduct : IDataProduct
    {
        public Dictionary<string, decimal> Price { get; set; } = [];
        public Dictionary<string, decimal> Quantity { get; set; } = [];
        public Dictionary<string, string> Availability { get; set; } = [];
        public Dictionary<string, decimal> Discount { get; set; } = [];
        public Dictionary<string, DateOnly> DiscountFrom { get; set; } = [];
        public Dictionary<string, DateOnly> DiscountTo { get; set; } = [];

        public List<string> Colisions { get; set; } = [];

        public void AddProductPrice(string productName, decimal price)
        {
            if (Price.ContainsKey(productName))
            {
                Colisions.Add("price " + productName);
                return;
            }
            Price.Add(productName, price);
        }

        public void AddProductAvailability(string productName, string availability)
        {
            if (Availability.ContainsKey(productName))
            {
                Colisions.Add("Availability " + productName);
                return;
            }
            Availability.Add(productName, availability);
        }

        public void AddProductQuantity(string productName, decimal quantity)
        {
            if (Quantity.ContainsKey(productName))
            {
                Colisions.Add("Quantity " + productName);
                return;
            }
            Quantity.Add(productName, quantity);
        }

        public void AddProductDiscount(string productName, decimal discount)
        {
            if (Discount.ContainsKey(productName))
            {
                Colisions.Add("Discount " + productName);
                return;
            }
            Discount.Add(productName, discount);
        }

        public void AddProductDiscountFrom(string productName, DateOnly discount)
        {
            if (DiscountFrom.ContainsKey(productName))
            {
                Colisions.Add("DiscountFrom " + productName);
                return;
            }
            DiscountFrom.Add(productName, discount);
        }

        public void AddProductDiscountTo(string productName, DateOnly discount)
        {
            if (DiscountTo.ContainsKey(productName))
            {
                Colisions.Add("DiscountTo " + productName);
                return;
            }
            DiscountTo.Add(productName, discount);
        }

        public void ClearData()
        {
            Price.Clear();
            Quantity.Clear();
            Availability.Clear();
            Discount.Clear();
            DiscountFrom.Clear();
            DiscountTo.Clear();
        }

        public Dictionary<string, List<string>> GetColisium()
        {
            if (Colisions.Count == 0) return [];
            var result = new Dictionary<string, List<string>>();

            foreach (var product in Colisions)
            {
                string[] strings = product.Split();
                if (!result.ContainsKey(strings[0]))
                    result[strings[0]] = [];
                result[strings[0]].Add(strings[1]);
            }
            return result;
        }
    }
}
