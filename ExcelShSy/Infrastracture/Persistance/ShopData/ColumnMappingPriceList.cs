namespace ExcelShSy.Infrastracture.Persistance.ShopData
{
    public static class ColumnMappingPriceList
    {
        public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Template = new Dictionary<string, IReadOnlyList<string>>
        {
            ["Article"] = new[] { "Article", "Артикул" },
            ["Price"] = new[] { "Price", "РРЦ, грн", "РРЦ грн", "Ціна" },
            ["ArticleComlect"] = new[] { "Article Comlect", "Артикул комплекта" },
            ["PriceComlect"] = new[] { "Price Comlect", "Ціна комплекта" },
            ["Availability"] = new[] { "Availability", "Наличие" },
            ["AvailabilityComplet"] = new[] { "Availability Complet", "Наличие комплекта" }
        };
    }
}