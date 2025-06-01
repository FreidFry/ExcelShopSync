using ExcelShSy.Infrastructure.Persistance.DefaultValues;

namespace ExcelShSy.Infrastructure.Persistance.ShopData.Mappings
{
    public static class ColumnMapping
    {
        public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Columns = new Dictionary<string, IReadOnlyList<string>>
        {
            [ColumnConstants.Article] = ["Артикул", "Код_товару", "Артикул*", "Article", "Артикул", "article"],
            [ColumnConstants.Price] = ["Цена", "Ціна", "Ціна*", "Price", "price"],
            [ColumnConstants.CompectArticle] = ["артикул комплекту", "ArticleComplect"],
            [ColumnConstants.CompectPrice] = ["ціна комплекту", "PriceComplect"],
            [ColumnConstants.PriceOld] = ["Старая цена", "Стара ціна"],
            [ColumnConstants.Quantity] = ["Количество", "Кількість", "Залишки"],
            [ColumnConstants.Availability] = ["Наличие", "Наявність", "Наявність*", "availability"],
            [ColumnConstants.Discount] = ["Скидка %", "Знижка"],
            [ColumnConstants.DiscountFrom] = ["Термін_дії_знижки_від"],
            [ColumnConstants.DiscountTo] = ["Дата и время окончания акции", "Термін_дії_знижки_до"]
        };
    }
}