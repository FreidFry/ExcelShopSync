using ExcelShSy.Infrastructure.Persistance.DefaultValues;

namespace ExcelShSy.Infrastructure.Persistance.ShopData.Mappings
{
    public static class ColumnMapping
    {
        public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Columns = new Dictionary<string, IReadOnlyList<string>>
        {
            [ColumnConstants.Article] = ["Артикул", "Код_товару", "Артикул*", "Article", "Артикул"],
            [ColumnConstants.ComplectArticle] = ["Артикул комплекту", "ArticleComplect"],
            [ColumnConstants.Price] = ["Цена", "Ціна", "Ціна*", "Price"],
            [ColumnConstants.ComplectPrice] = ["Цена комплекта", "Ціна комплекту", "PriceComplect"],
            [ColumnConstants.Quantity] = ["Количество", "Кількість", "Залишки", "Quantity"],
            [ColumnConstants.ComplectQuantity] = ["Количество комплекта", "Кількість комплекту", "QuantityComplect"],
            [ColumnConstants.Availability] = ["Наличие", "Наявність", "Наявність*", "Availability"],
            [ColumnConstants.ComplectAvailability] = ["Наличие комплекта", "Наявність комплекту", "AvailabilityComplect"],
            [ColumnConstants.PriceOld] = ["Старая цена", "Стара ціна", "OldPrice"],
            [ColumnConstants.Discount] = ["Скидка %", "Знижка", "Discount"],
            [ColumnConstants.DiscountFrom] = ["Термін_дії_знижки_від", "Дата начала скидки", "Дата початку знижки", "DiscountDateStart"],
            [ColumnConstants.DiscountTo] = ["Дата и время окончания акции", "Термін_дії_знижки_до", "Дата окончания скидки", "Дата кінця знижки", "DiscuontDateEnd"]
        };
    }
}