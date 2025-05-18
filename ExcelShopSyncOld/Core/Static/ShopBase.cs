using static ExcelShopSync.Core.Static.ColumnKeys;

namespace ExcelShopSync.Core.Static
{
    public static class ShopBase
    {
        static public Dictionary<string, List<string>> Columns = new()
        {
            { Article, new List<string> { "Артикул", "Код_товару", "Артикул*", "Article", "Артикул", "article" } },
            { Price, new List<string> { "Цена", "Ціна", "Ціна*", "Price", "price" } },
            { CompectArticle, new List<string> { "артикул комплекту", "ArticleComplect" } },
            { CompectPrice, new List<string> { "ціна комплекту", "PriceComplect" } },
            { PriceOld, new List<string> { "Старая цена", "Стара ціна" } },
            { Quantity, new List<string> { "Количество", "Кількість", "Залишки" } },
            { Availability, new List<string> { "Наличие", "Наявність", "Наявність*", "availability" } },
            { Discount, new List<string> { "Скидка %", "Знижка" } },
            { DiscountFrom, new List<string> { "Термін_дії_знижки_від" } },
            { DiscountTo, new List<string> { "Дата и время окончания акции", "Термін_дії_знижки_до" } }
        };
    }
}