using ExcelShSy.Infrastructure.Persistence.DefaultValues;

namespace ExcelShSy.Infrastructure.Persistence.ShopData.Mappings
{
    public static class ColumnMappingPriceList
    {
        public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Template = new Dictionary<string, IReadOnlyList<string>>
        {
            [ColumnConstants.Article] = ["Article", "Артикул"],
            [ColumnConstants.Price] = ["Price", "Ціна", "Цена"],
            [ColumnConstants.Availability] = ["Availability", "Наличие", "Наявність"],
            [ColumnConstants.ComplectArticle] = ["ArticleComplect", "Артикул комплекта", "Артикул комплекту"],
            [ColumnConstants.ComplectPrice] = ["PriceComplect", "Цена комплекта", "Ціна комплекту"],
            [ColumnConstants.ComplectAvailability] = ["AvailabilityComplect", "Наличие комплекта", "Наявність комплекту"]
        };
    }
}