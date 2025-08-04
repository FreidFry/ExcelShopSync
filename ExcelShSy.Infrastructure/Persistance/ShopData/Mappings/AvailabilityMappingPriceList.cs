using ExcelShSy.Infrastructure.Persistance.DefaultValues;

namespace ExcelShSy.Infrastructure.Persistance.ShopData.Mappings
{
    internal class AvailabilityMappingPriceList
    {
        public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Template = new Dictionary<string, IReadOnlyList<string>>
        {
            [AvailabilityConstant.OnOrder] = ["Под заказ", "Під замовлення", "OnOrder"],
            [AvailabilityConstant.ReadyToGo] = ["Готов к отправке", "Готове до відправки", "ReadyToGo"],
            [AvailabilityConstant.OutOfStock] = ["Нет в наличии", "Немає у наявності", "OutOfStock"],
            [AvailabilityConstant.InStock] = ["В наличии", "У наявності", "InStock"],
        };
    }
}
