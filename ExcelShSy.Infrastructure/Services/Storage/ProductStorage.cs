using ExcelShSy.Core.Interfaces.Storage;

namespace ExcelShSy.Infrastructure.Services.Storage
{
    /// <summary>
    /// In-memory implementation of <see cref="IProductStorage"/> used to cache product data fetched from Excel files.
    /// </summary>
    public class ProductStorage : IProductStorage
    {
        /// <inheritdoc />
        public HashSet<string> Articles { get; set; } = [];
        /// <inheritdoc />
        public Dictionary<string, decimal> Price { get; set; } = [];
        /// <inheritdoc />
        public Dictionary<string, decimal> Quantity { get; set; } = [];
        /// <inheritdoc />
        public Dictionary<string, string> Availability { get; set; } = [];
        /// <inheritdoc />
        public Dictionary<string, decimal> Discount { get; set; } = [];
        /// <inheritdoc />
        public Dictionary<string, DateTime> DiscountFrom { get; set; } = [];
        /// <inheritdoc />
        public Dictionary<string, DateTime> DiscountTo { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of duplicate entries encountered while adding product data.
        /// </summary>
        public List<string> Collisions { get; set; } = [];

        /// <inheritdoc />
        public void AddProductPrice(string productName, decimal price)
        {
            if (!Price.TryAdd(productName, price))
            {
                Collisions.Add("price " + productName);
            }
        }

        /// <inheritdoc />
        public void AddProductAvailability(string productName, string availability)
        {
            if (!Availability.TryAdd(productName, availability))
            {
                Collisions.Add("Availability " + productName);
            }
        }

        /// <inheritdoc />
        public void AddProductQuantity(string productName, decimal quantity)
        {
            if (!Quantity.TryAdd(productName, quantity))
            {
                Collisions.Add("Quantity " + productName);
            }
        }

        /// <inheritdoc />
        public void AddProductDiscount(string productName, decimal discount)
        {
            if (!Discount.TryAdd(productName, discount))
            {
                Collisions.Add("Discount " + productName);
            }
        }

        /// <inheritdoc />
        public void AddProductDiscountFrom(string productName, DateTime discount)
        {
            if (!DiscountFrom.TryAdd(productName, discount))
            {
                Collisions.Add("DiscountFrom " + productName);
            }
        }

        /// <inheritdoc />
        public void AddProductDiscountTo(string productName, DateTime discount)
        {
            if (!DiscountTo.TryAdd(productName, discount))
            {
                Collisions.Add("DiscountTo " + productName);
            }
        }

        /// <inheritdoc />
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

        /// <summary>
        /// Builds a dictionary grouping collision entries by their type.
        /// </summary>
        /// <returns>A dictionary keyed by collision type with the associated product identifiers.</returns>
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

        /// <inheritdoc />
        public void AddProductArticle(string article)
        {
            Articles.Add(article);
        }
    }
}
