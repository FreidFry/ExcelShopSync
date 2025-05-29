namespace ExcelShSy.Properties
{
    public static class GlobalSettings
    {
        public static bool IsRound { get; set; } = false;
        public static bool SyncPrice { get; set; } = false;
        public static bool SyncQuantity { get; set; } = false;
        public static bool SyncAvailability { get; set; } = false;
        public static bool SyncDiscount { get; set; } = false;
        public static bool IncreasePricePercent { get; set; } = false;
        public static bool DiscountDate { get; set; } = false;
        public static DateOnly MinDateActually { get; set; } = new DateOnly(2025, 1, 1);

        public static decimal priceIncreasePercentage { get;set; }
    }
}
