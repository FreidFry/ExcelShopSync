using ExcelShSy.Infrastructure.Persistance.DefaultValues;

namespace ExcelShSy.Infrastructure.Persistance.ShopData.Mappings
{
    public static class ColumnMappingPriceList
    {
        public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Template = new Dictionary<string, IReadOnlyList<string>>
        {
            [ColumnConstants.Article] = ["Article", "Артикул"],
            [ColumnConstants.Price] = ["Price", "РРЦ, грн", "РРЦ грн", "Ціна"],
            [ColumnConstants.CompectArticle] = ["Article Comlect", "Артикул комплекта"],
            [ColumnConstants.CompectPrice] = ["Price Comlect", "Ціна комплекта"],
            [ColumnConstants.Availability] = ["Availability", "Наличие"],
            [ColumnConstants.CompectAvailability] = ["Availability Complet", "Наличие комплекта"]
        };
    }
}