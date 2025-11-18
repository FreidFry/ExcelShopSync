using ExcelShSy.Infrastructure.Persistence.DefaultValues;

namespace ExcelShSy.Infrastructure.Persistence.ShopData.Mappings
{
    /// <summary>
    /// Default availability value mappings for price list imports.
    /// </summary>
    internal static class AvailabilityMappingPriceList
    {
        /// <summary>
        /// Gets the mapping of standardized availability keys to possible textual representations.
        /// </summary>
        public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Template = new Dictionary<string, IReadOnlyList<string>>
        {
            [AvailabilityConstant.OnOrder] = ["Под заказ", "Під замовлення", "OnOrder"],
            [AvailabilityConstant.ReadyToGo] = ["Готов к отправке", "Готове до відправки", "ReadyToGo"],
            [AvailabilityConstant.OutOfStock] = ["Нет в наличии", "Немає у наявності", "OutOfStock"],
            [AvailabilityConstant.InStock] = ["В наличии", "У наявності", "InStock"],
        };
    }
}
