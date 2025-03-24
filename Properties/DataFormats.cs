using static ExcelShopSync.Modules.Shops;

namespace ExcelShopSync.Properties
{
    public static class DataFormats
    {
        public static readonly Dictionary<string, string> formats = new()
        {
            {Horoshop, "dd-MM-yyyy HH:mm:ss"},
            {Unknown, "dd.MM.yy HH:mm:ss" },
        };
    }
}