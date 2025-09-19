using ExcelShSy.Core.Interfaces.Shop;

namespace ExcelShSy.Core.Interfaces.Storage
{
    public interface IColumnMappingStorage
    {
        public Dictionary<string, List<string>> Columns { get; }

        void AddColumn(string key, List<string> values);
        void AddColumn(string key, string value);
        void AddColumn(IShopTemplate shop);
    }
}