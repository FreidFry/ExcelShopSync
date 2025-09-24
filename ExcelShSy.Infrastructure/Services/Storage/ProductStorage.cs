using ExcelShSy.Core.Interfaces.Storage;

namespace ExcelShSy.Infrastructure.Services.Storage
{
    public class ProductStorage : IProductStorage
    {
        public HashSet<string> Articles { get; set; } = [];
        public Dictionary<string, decimal> Price { get; set; } = [];
        public Dictionary<string, decimal> Quantity { get; set; } = [];
        public Dictionary<string, string> Availability { get; set; } = [];
        public Dictionary<string, decimal> Discount { get; set; } = [];
        public Dictionary<string, DateOnly> DiscountFrom { get; set; } = [];
        public Dictionary<string, DateOnly> DiscountTo { get; set; } = [];

        public List<string> Collisions { get; set; } = [];

        public void AddProductPrice(string productName, decimal price)
        {
            if (!Price.TryAdd(productName, price))
            {
                Collisions.Add("price " + productName);
            }
        }

        public void AddProductAvailability(string productName, string availability)
        {
            if (!Availability.TryAdd(productName, availability))
            {
                Collisions.Add("Availability " + productName);
            }
        }

        public void AddProductQuantity(string productName, decimal quantity)
        {
            if (!Quantity.TryAdd(productName, quantity))
            {
                Collisions.Add("Quantity " + productName);
            }
        }

        public void AddProductDiscount(string productName, decimal discount)
        {
            if (!Discount.TryAdd(productName, discount))
            {
                Collisions.Add("Discount " + productName);
            }
        }

        public void AddProductDiscountFrom(string productName, DateOnly discount)
        {
            if (!DiscountFrom.TryAdd(productName, discount))
            {
                Collisions.Add("DiscountFrom " + productName);
            }
        }

        public void AddProductDiscountTo(string productName, DateOnly discount)
        {
            if (!DiscountTo.TryAdd(productName, discount))
            {
                Collisions.Add("DiscountTo " + productName);
            }
        }

        public void ClearData()
        {
            Articles.Clear();
            Price.Clear();
            Quantity.Clear();
            Availability.Clear();
            Discount.Clear();
            DiscountFrom.Clear();
            DiscountTo.Clear();
            
            Collisions.Clear();
        }

        public Dictionary<string, List<string>> GetCollisions()
        {
            if (Collisions.Count == 0) return [];
            var result = new Dictionary<string, List<string>>();

            foreach (var product in Collisions)
            {
                var strings = product.Split();
                if (!result.ContainsKey(strings[0]))
                    result[strings[0]] = [];
                result[strings[0]].Add(strings[1]);
            }
            return result;
        }

        public void AddProductArticle(string article)
        {
            Articles.Add(article);
        }
    }
}
