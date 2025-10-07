using System.Diagnostics.CodeAnalysis;
using ExcelShSy.Infrastructure.Persistence.ShopData.Mappings;

namespace ExcelShSy.Infrastructure.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool IsNullOrEmpty<TKey, TValue>([NotNullWhen(false)] this IDictionary<TKey, TValue>? dict) =>
            dict == null || dict.Count == 0;

        private static Dictionary<string, int>? GetHeaderMapFromTemplate(this IDictionary<string, int>? range, IReadOnlyDictionary<string, IReadOnlyList<string>>? template)
        {
            if (range == null) return null;

            var result = template?.SelectMany(pair => pair.Value, (pair, name) => new { pair.Key, Name = name })
                .Where(x => range.ContainsKey(x.Name))
                .GroupBy(x => x.Key)
                .ToDictionary(g => g.Key, g => range[g.First().Name]);

            return result?.Count > 0 ? result : null;
        }

        private static int GetColumnFromRange(this IDictionary<string, int>? range, string columnName)
        {
            var priceTemplate = ColumnMappingPriceList.Template;
            var mapping = range.GetHeaderMapFromTemplate(priceTemplate);

            if (mapping == null) return 0;
            
            mapping.TryGetValue(columnName, out var result);
            return result;
        }

        public static int GetColumnFromRange(this int oldVal, Dictionary<string, int>? range, string columnName, ref bool isHeader)
        {
            var value = range.GetColumnFromRange(columnName);
            if (value <= 0) return oldVal;
            
            isHeader = true;
            return value;
        }
    }
}
