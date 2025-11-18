namespace ExcelShSy.Core.Interfaces.Shop
{
    /// <summary>
    /// Represents a mapping template that describes how shop-specific columns map to logical product fields.
    /// </summary>
    public interface IShopTemplate
    {
        /// <summary>
        /// Gets or sets the display name of the shop template.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets headers that could not be mapped to known product fields.
        /// </summary>
        List<string> UnmappedHeaders { get; set; }

        /// <summary>
        /// Gets or sets the mapping of availability values to their standardized representation.
        /// </summary>
        Dictionary<string, string?> AvailabilityMap { get; set; }

        /// <summary>
        /// Gets or sets the data format string used when parsing values for this template.
        /// </summary>
        string? DataFormat { get; set; }

        /// <summary>
        /// Gets or sets the column name that contains product article identifiers.
        /// </summary>
        string? Article { get; set; }

        /// <summary>
        /// Gets or sets the column name that contains product price values.
        /// </summary>
        string? Price { get; set; }

        /// <summary>
        /// Gets or sets the column name that contains availability information.
        /// </summary>
        string? Availability { get; set; }

        /// <summary>
        /// Gets or sets the column name that contains product quantities.
        /// </summary>
        string? Quantity { get; set; }

        /// <summary>
        /// Gets or sets the column name that contains discount values.
        /// </summary>
        string? Discount { get; set; }

        /// <summary>
        /// Gets or sets the column name that contains the discount start date.
        /// </summary>
        string? DiscountDateStart { get; set; }

        /// <summary>
        /// Gets or sets the column name that contains the discount end date.
        /// </summary>
        string? DiscountDateEnd { get; set; }

        /// <summary>
        /// Gets or sets the column name that contains the product's old price.
        /// </summary>
        string? OldPrice { get; set; }

        /// <summary>
        /// Creates a copy of this shop template.
        /// </summary>
        /// <returns>A new <see cref="IShopTemplate"/> instance with the same properties.</returns>
        public IShopTemplate Clone();
    }
}
