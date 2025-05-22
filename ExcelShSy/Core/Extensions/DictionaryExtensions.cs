using ExcelShSy.Core.Interfaces;
using ExcelShSy.Infrastracture.Persistance.DefaultValues;
using ExcelShSy.Infrastracture.Persistance.ShopData;

namespace ExcelShSy.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool IsNullOrEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dict)
            where TKey : notnull =>
            dict == null || dict.Count == 0;

        public static bool HasRequiredKeys(this Dictionary<string, int> dict, params string[] keys) =>
            keys.All(key => dict.ContainsKey(key));

        public static Dictionary<string, int>? GetHeaderMapFromTemplate(this IDictionary<string, int>? range, IReadOnlyDictionary<string, IReadOnlyList<string>>? template, int row)
        {
            if (range == null || range.Count < 2) return [];

            var result = template
                .SelectMany(pair => pair.Value, (pair, name) => new { pair.Key, Name = name })
                .Where(x => range.ContainsKey(x.Name))
                .GroupBy(x => x.Key)
                .ToDictionary(g => g.Key, g => range[g.First().Name]);

            return result.Count > 0 ? result : null;
        }

        public static HeaderMap? GetIndefyPriceHeader(this IDictionary<string, int>? range, int row)
        {
            var priceTemplate = ColumnMappingPriceList.Template;
            var mapping = range.GetHeaderMapFromTemplate(priceTemplate, row);

            if (mapping == null || mapping.Count < 2)
                return null;

            mapping.TryGetValue(ColumnConstants.Article, out int articleCol);
            mapping.TryGetValue(ColumnConstants.Price, out int priceCol);
            mapping.TryGetValue(ColumnConstants.Availability, out int availabilityCol);

            return new HeaderMap(articleCol, priceCol, availabilityCol);
        }

        public static HeaderMap? GetIndefyPriceHeaderComplect(this IDictionary<string, int>? range, int row)
        {
            var priceTemplate = ColumnMappingPriceList.Template;
            var mapping = range.GetHeaderMapFromTemplate(priceTemplate, row);

            if (mapping == null || mapping.Count < 2)
                return null;

            mapping.TryGetValue(ColumnConstants.CompectArticle, out int articleCol);
            mapping.TryGetValue(ColumnConstants.CompectPrice, out int priceCol);
            mapping.TryGetValue(ColumnConstants.CompectAvailability, out int availabilityCol);

            return new HeaderMap(articleCol, priceCol, availabilityCol);
        }
    }
}
