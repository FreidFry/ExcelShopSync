using ExcelShSy.Core.Interfaces.Shop;

using static ExcelShSy.Infrastracture.Persistance.DefaultValues.AvailabilityConstant;

namespace ExcelShSy.Infrastracture.Persistance.ShopData.Datas
{
    public record IbudData : IShopTemplate
    {
        public IReadOnlyList<string> Columns =>
        [
            "id",
            "article",
            "name",
            "price",
            "sale_price",
            "currency",
            "measure",
            "availability",
            "min_order",
            "max_order",
            "producer"
        ];

        public IReadOnlyDictionary<string, string> Availability => new Dictionary<string, string>
        {
            { InStock, "В наличии" },
            { OutOfStock, "Нет в наличии" },
            { OnOrder, "Под заказ" },
            { ReadyToGo, "в наявності" },
        };
    }
}
