using ExcelShSy.Infrastructure.Persistance.ShopData.Mappings;

using System.Diagnostics.CodeAnalysis;

namespace ExcelShSy.Infrastructure.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool IsNullOrEmpty<TKey, TValue>([NotNullWhen(false)] this IDictionary<TKey, TValue>? dict) =>
            dict == null || dict.Count == 0;

        public static bool HasRequiredKeys(this Dictionary<string, int> dict, params string[] keys) =>
            keys.All(key => dict.ContainsKey(key));

        public static Dictionary<string, int>? GetHeaderMapFromTemplate(this IDictionary<string, int>? range, IReadOnlyDictionary<string, IReadOnlyList<string>>? template)
        {
            if (range == null) return [];

            var result = template
                .SelectMany(pair => pair.Value, (pair, name) => new { pair.Key, Name = name })
                .Where(x => range.ContainsKey(x.Name))
                .GroupBy(x => x.Key)
                .ToDictionary(g => g.Key, g => range[g.First().Name]);

            return result.Count > 0 ? result : null;
        }

        public static int GetColumnFromRange(this IDictionary<string, int>? range, string columnName)
        {
            var priceTemplate = ColumnMappingPriceList.Template;
            var mapping = range.GetHeaderMapFromTemplate(priceTemplate);

            if (mapping != null)
            {
                mapping.TryGetValue(columnName, out int result);
                return result;
            }
            return 0;
        }

        public static int GetColumnFromRange(this int oldVal, Dictionary<string, int>? range, string ColumnName)
        {
            int value = range.GetColumnFromRange(ColumnName);
            if (value > 0)
                return value;
            return oldVal;
        }
    }
}
