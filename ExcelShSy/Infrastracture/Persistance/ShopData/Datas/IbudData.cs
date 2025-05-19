using ExcelShSy.Core.Interfaces;
using static ExcelShSy.Infrastracture.Persistance.ShopData.AvailabilityMapping;

namespace ExcelShSy.Infrastracture.Persistance.ShopData.Datas
{
    public record IbudData : IShopTemplate
    {
        public IReadOnlyList<string> columns => new List<string>
        {
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
        };

        public IReadOnlyDictionary<string, string> Availability => new Dictionary<string, string>
        {
            { InStock, "В наличии" },
            { OutOfStock, "Нет в наличии" },
            { OnOrder, "Под заказ" },
            { ReadyToGo, "в наявності" },
        };
    }
}
