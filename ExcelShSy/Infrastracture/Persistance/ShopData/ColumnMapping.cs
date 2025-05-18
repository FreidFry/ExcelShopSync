using ExcelShSy.Infrastracture.Persistance.DefaultValues;

namespace ExcelShSy.Infrastracture.Persistance.ShopData
{
    public static class ColumnMapping
    {
        public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Columns = new Dictionary<string, IReadOnlyList<string>>
        {
            [ColumnConstants.Article] = new[] { "Артикул", "Код_товару", "Артикул*", "Article", "Артикул", "article" },
            [ColumnConstants.Price] = new[] { "Цена", "Ціна", "Ціна*", "Price", "price" },
            [ColumnConstants.CompectArticle] = new[] { "артикул комплекту", "ArticleComplect" },
            [ColumnConstants.CompectPrice] = new[] { "ціна комплекту", "PriceComplect" },
            [ColumnConstants.PriceOld] = new[] { "Старая цена", "Стара ціна" },
            [ColumnConstants.Quantity] = new[] { "Количество", "Кількість", "Залишки" },
            [ColumnConstants.Availability] = new[] { "Наличие", "Наявність", "Наявність*", "availability" },
            [ColumnConstants.Discount] = new[] { "Скидка %", "Знижка" },
            [ColumnConstants.DiscountFrom] = new[] { "Термін_дії_знижки_від" },
            [ColumnConstants.DiscountTo] = new[] { "Дата и время окончания акции", "Термін_дії_знижки_до" }
        };
    }
}