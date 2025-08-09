
using ExcelShSy.Core.Interfaces.Shop;

namespace ExcelShSy.Infrastructure.Services.Storage
{
    public interface IColumnMappingStorage
    {
        public Dictionary<string, List<string>> Columns { get; }

        void AddColumn(string key, List<string> values);
        void AddColumn(string key, string value);
        void AddColumn(IShopTemplate shop);
    }
}