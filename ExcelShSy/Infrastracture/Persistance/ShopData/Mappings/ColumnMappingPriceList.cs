using ExcelShSy.Infrastracture.Persistance.DefaultValues;

namespace ExcelShSy.Infrastracture.Persistance.ShopData
{
    public static class ColumnMappingPriceList
    {
        public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Template = new Dictionary<string, IReadOnlyList<string>>
        {
            [ColumnConstants.Article] = new[] { "Article", "Артикул" },
            [ColumnConstants.Price] = new[] { "Price", "РРЦ, грн", "РРЦ грн", "Ціна" },
            [ColumnConstants.CompectArticle] = new[] { "Article Comlect", "Артикул комплекта" },
            [ColumnConstants.CompectPrice] = new[] { "Price Comlect", "Ціна комплекта" },
            [ColumnConstants.Availability] = new[] { "Availability", "Наличие" },
            [ColumnConstants.CompectAvailability] = new[] { "Availability Complet", "Наличие комплекта" }
        };
    }
}