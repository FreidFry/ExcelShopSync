using static ExcelShopSync.Modules.ColumnKeys;


namespace ExcelShopSync.Properties
{
    class ShopBase
    {
        public Dictionary<string, List<string>> Columns = new()
        {
            { Article, new List<string> { "Артикул" } },
            { Price, new List<string> { "Цена" } },
            { PriceOld, new List<string> { "Старая цена" } },
            { Quantity, new List<string> { "Количество" } },
            { Availability, new List<string> { "Наличие" } },
            { Discount, new List<string> { "Скидка %" } },
            { DiscountFrom, new List<string> { } },
            { DiscountTo, new List<string> { "Дата и время окончания акции" } }
        };
    }
}