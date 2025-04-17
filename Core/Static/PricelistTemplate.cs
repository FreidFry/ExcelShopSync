namespace ExcelShopSync.Core.Static
{
    public static class PricelistTemplate
    {
        public static Dictionary<string, List<string>> pricelistTemplate = new()
        {
            { "Article", new List<string> {"Article", "Артикул" } },
            { "Price", new List<string> {"Price", "РРЦ, грн", "РРЦ грн", "Ціна" } },
            { "ArticleComlect", new List <string> { "Article Comlect", "Артикул комплекта" } },
            { "PriceComlect", new List <string> { "Price Comlect", "Ціна комплекта" } },
            { "Availability", new List <string> { "Availability", "Наличие" }},
            { "AvailabilityComplet", new List <string> { "Availability Complet", "Наличие комплекта" }},
        };
    }
}
