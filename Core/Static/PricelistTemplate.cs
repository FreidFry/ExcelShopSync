namespace ExcelShopSync.Core.Static
{
    public static class PricelistTemplate
    {
        public static Dictionary<string, List<string>> pricelistTemplate = new()
        {
            { "Article", new List<string> {"Article", "Артикул" } },
            { "Price", new List<string> {"Price", "РРЦ, грн" } },
            { "ArticleComlect", new List <string> { "ArticleComlect", "Артикул комплекта" } },
            { "PriceComlect", new List <string> { "PriceComlect", "Цена комплекта" } },
            { "Availability", new List <string> { "Availability", "Наличие" }},
            { "AvailabilityComlect", new List <string> { "AvailabilityComlect", "Наличие комплекта" }},
        };
    }
}
