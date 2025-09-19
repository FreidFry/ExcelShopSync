using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Persistance.DefaultValues;

namespace ExcelShSy.Infrastructure.Services.Storage
{
    public class ColumnMappingStorage : IColumnMappingStorage
    {
        public Dictionary<string, List<string>> Columns { get; private set; } = [];

        public void AddColumn(string key, List<string> values)
        {
            if (Columns.ContainsKey(key))
            {
                Columns[key].AddRange(values);
            }
            else
            {
                Columns[key] = new List<string>(values);
            }
        }

        public void AddColumn(string key, string? value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            if (Columns.ContainsKey(key))
                Columns[key].Add(value);
            else
                Columns[key] = [value];
        }

        public void AddColumn(IShopTemplate shop)
        {
            AddColumn(ColumnConstants.Article, shop.Article);
            AddColumn(ColumnConstants.Price, shop.Price);
            AddColumn(ColumnConstants.PriceOld, shop.OldPrice);
            AddColumn(ColumnConstants.Quantity, shop.Quantity);
            AddColumn(ColumnConstants.Availability, shop.Availability);
            AddColumn(ColumnConstants.Discount, shop.Discount);
            AddColumn(ColumnConstants.DiscountFrom, shop.DiscountDateStart);
            AddColumn(ColumnConstants.DiscountTo, shop.DiscountDateEnd);
        }
    }
}