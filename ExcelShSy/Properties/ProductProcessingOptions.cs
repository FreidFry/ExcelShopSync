namespace ExcelShSy.Core.Properties
{
    public static class ProductProcessingOptions
    {
        public static bool ShouldRoundPrices { get; set; } = false;
        public static bool ShouldSyncPrices { get; set; } = false;
        public static bool ShouldSyncQuantities { get; set; } = false;
        public static bool ShouldSyncAvailability { get; set; } = false;
        public static bool ShouldSyncDiscounts { get; set; } = false;
        public static bool ShouldIncreasePrices { get; set; } = false;
        public static bool ShouldSyncDiscountDate { get; set; } = false;
        public static DateOnly MinDateActually { get; set; } = new DateOnly(2025, 1, 1);
        public static decimal priceIncreasePercentage { get;set; }
        public static bool ShouldFindMissingProducts { get; set; } = false;
    }
}
