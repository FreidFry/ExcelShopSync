namespace ExcelShSy.Properties
{
    public static class GlobalSettings
    {
        public static bool IsRound { get; set; } = false;
        public static bool SyncPrice { get; set; } = false;
        public static bool SyncQuantity { get; set; } = false;
        public static bool SyncAvailability { get; set; } = false;
        public static bool SyncDiscount { get; set; } = false;
        public static bool DiscountDate { get; set; } = false;
        public static int DefaultDiscountEndTimeOffSet { get; set; } = 14;
        public static DateOnly MinDateActually { get; set; } = new DateOnly(2025, 1, 1);
        public static DateOnly DiscountStartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public static DateOnly DiscountEndDate { get; set; }

        public static DateOnly DateNow { get; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
