namespace ExcelShopSync.Properties
{
    class ShopBase
    {
        public static class ColumnKeys
        {
            public const string Article = "Article";
            public const string Price = "Price";
            public const string PriceOld = "PriceOld";
            public const string Quantity = "Quantity";
            public const string Availability = "Availability";
            public const string Discount = "Discount";
            public const string DiscountFrom = "DiscountFrom";
            public const string DiscountTo = "DiscountTo";
        }

        public Dictionary<string, List<string>> Columns = new()
        {
            { ColumnKeys.Article, new List<string> { "Артикул" } },
            { ColumnKeys.Price, new List<string> { "Цена" } },
            { ColumnKeys.PriceOld, new List<string> { "Старая цена" } },
            { ColumnKeys.Quantity, new List<string> { "Количество" } },
            { ColumnKeys.Availability, new List<string> { "Наличие" } },
            { ColumnKeys.Discount, new List<string> { "Скидка %" } },
            { ColumnKeys.DiscountFrom, new List<string> { } },
            { ColumnKeys.DiscountTo, new List<string> { "Дата и время окончания акции" } }
        };

        public static class DataFormats
        {
            public const string Horoshop = "Хорошоп";
        }

        public static Dictionary<string, string> formats = new()
        {
            {DataFormats.Horoshop, "dd.MM.yyyy HH:mm:ss"}
        };
    }
}