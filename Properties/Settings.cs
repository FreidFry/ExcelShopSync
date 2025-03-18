namespace ExcelShopSync.Properties
{
    public class Settings
    {
        public static int DefaultDiscount { get; private set; } = 10;
        public static double DefaultFakeDiscount { get; private set; } = 5.0;
        public static int DefaultTimeOffset { get; private set; } = 14;
        public static TimeOnly DefaultTime { get; private set; } = new TimeOnly(14, 0);


        public static readonly string settingsFilePath = "settings.json";
        
    }
}
