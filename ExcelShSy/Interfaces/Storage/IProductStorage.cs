namespace ExcelShSy.Core.Interfaces.Storage
{
    /// <summary>
    /// Defines a contract for storing and managing product data collected from external sources.
    /// </summary>
    public interface IProductStorage
    {
        /// <summary>
        /// Gets or sets the collection of product article identifiers.
        /// </summary>
        HashSet<string> Articles { get; set; }

        /// <summary>
        /// Gets or sets the product price lookup keyed by article or product identifier.
        /// </summary>
        Dictionary<string, decimal> Price { get; set; }

        /// <summary>
        /// Gets or sets the product quantity lookup keyed by article or product identifier.
        /// </summary>
        Dictionary<string, decimal> Quantity { get; set; }

        /// <summary>
        /// Gets or sets the product availability lookup keyed by article or product identifier.
        /// </summary>
        Dictionary<string, string> Availability { get; set; }

        /// <summary>
        /// Gets or sets the product discount lookup keyed by article or product identifier.
        /// </summary>
        Dictionary<string, decimal> Discount { get; set; }

        /// <summary>
        /// Gets or sets the lookup of discount start dates keyed by article or product identifier.
        /// </summary>
        Dictionary<string, DateTime> DiscountFrom { get; set; }

        /// <summary>
        /// Gets or sets the lookup of discount end dates keyed by article or product identifier.
        /// </summary>
        Dictionary<string, DateTime> DiscountTo { get; set; }

        /// <summary>
        /// Adds or updates the price for the specified product.
        /// </summary>
        /// <param name="productName">The identifier of the product to update.</param>
        /// <param name="price">The product price value.</param>
        void AddProductPrice(string productName, decimal price);

        /// <summary>
        /// Adds or updates the availability state for the specified product.
        /// </summary>
        /// <param name="productName">The identifier of the product to update.</param>
        /// <param name="availability">The availability status to store.</param>
        void AddProductAvailability(string productName, string availability);

        /// <summary>
        /// Adds or updates the quantity for the specified product.
        /// </summary>
        /// <param name="productName">The identifier of the product to update.</param>
        /// <param name="quantity">The quantity value to store.</param>
        void AddProductQuantity(string productName, decimal quantity);

        /// <summary>
        /// Adds or updates the discount value for the specified product.
        /// </summary>
        /// <param name="productName">The identifier of the product to update.</param>
        /// <param name="discount">The discount value to store.</param>
        void AddProductDiscount(string productName, decimal discount);

        /// <summary>
        /// Adds or updates the discount start date for the specified product.
        /// </summary>
        /// <param name="productName">The identifier of the product to update.</param>
        /// <param name="discount">The discount start date to store.</param>
        void AddProductDiscountFrom(string productName, DateTime discount);

        /// <summary>
        /// Adds or updates the discount end date for the specified product.
        /// </summary>
        /// <param name="productName">The identifier of the product to update.</param>
        /// <param name="discount">The discount end date to store.</param>
        void AddProductDiscountTo(string productName, DateTime discount);

        /// <summary>
        /// Adds a product article identifier to the storage.
        /// </summary>
        /// <param name="article">The article identifier to add.</param>
        void AddProductArticle(string article);

        /// <summary>
        /// Clears all cached product data.
        /// </summary>
        void ClearData();
    }
}
