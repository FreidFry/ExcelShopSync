namespace ExcelShSy.Core.Properties
{
    /// <summary>
    /// Holds runtime-configurable options that control how products are processed and synchronized.
    /// </summary>
    public static class ProductProcessingOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether product prices should be rounded during processing.
        /// </summary>
        public static bool ShouldRoundPrices { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether product prices should be synchronized with the external source.
        /// </summary>
        public static bool ShouldSyncPrices { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether product quantities should be synchronized with the external source.
        /// </summary>
        public static bool ShouldSyncQuantities { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether product availability flags should be synchronized.
        /// </summary>
        public static bool ShouldSyncAvailability { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether discount values should be synchronized.
        /// </summary>
        public static bool ShouldSyncDiscounts { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether prices should be increased by a configured percentage.
        /// </summary>
        public static bool ShouldIncreasePrices { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether discount date fields should be updated.
        /// </summary>
        public static bool ShouldSyncDiscountDate { get; set; } = false;

        /// <summary>
        /// Gets or sets the minimum date considered valid when synchronizing discount periods.
        /// </summary>
        public static DateTime MinDateActually { get; set; } = new DateTime(2025, 1, 1);

        /// <summary>
        /// Gets or sets the percentage by which prices should be increased when <see cref="ShouldIncreasePrices"/> is enabled.
        /// </summary>
        public static decimal priceIncreasePercentage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether missing products should be detected during synchronization.
        /// </summary>
        public static bool ShouldFindMissingProducts { get; set; } = false;
    }
}
