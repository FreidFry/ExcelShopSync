using System.Diagnostics.CodeAnalysis;
using ExcelShSy.Infrastructure.Persistence.ShopData.Mappings;

namespace ExcelShSy.Infrastructure.Extensions
{
    /// <summary>
    /// Provides helper methods for working with dictionaries of header mappings.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Determines whether the dictionary is null or contains no elements.
        /// </summary>
        /// <param name="dict">The dictionary to inspect.</param>
        /// <returns><c>true</c> if the dictionary is null or empty; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrEmpty<TKey, TValue>([NotNullWhen(false)] this IDictionary<TKey, TValue>? dict) =>
            dict == null || dict.Count == 0;

        /// <summary>
        /// Generates a mapping from logical column keys to column indexes based on a template.
        /// </summary>
        /// <param name="range">The discovered headers.</param>
        /// <param name="template">The template that maps logical keys to possible aliases.</param>
        /// <returns>The resulting mapping, or <c>null</c> if no matches were found.</returns>
        private static Dictionary<string, int>? GetHeaderMapFromTemplate(this IDictionary<string, int>? range, IReadOnlyDictionary<string, IReadOnlyList<string>>? template)
        {
            if (range == null) return null;

            var result = template?.SelectMany(pair => pair.Value, (pair, name) => new { pair.Key, Name = name })
                .Where(x => range.ContainsKey(x.Name))
                .GroupBy(x => x.Key)
                .ToDictionary(g => g.Key, g => range[g.First().Name]);

            return result?.Count > 0 ? result : null;
        }

        /// <summary>
        /// Looks up a column index using the default price list template.
        /// </summary>
        /// <param name="range">The header dictionary to search.</param>
        /// <param name="columnName">The logical column name.</param>
        /// <returns>The column index or zero if not found.</returns>
        private static int GetColumnFromRange(this IDictionary<string, int>? range, string columnName)
        {
            var priceTemplate = ColumnMappingPriceList.Template;
            var mapping = range.GetHeaderMapFromTemplate(priceTemplate);

            if (mapping == null) return 0;
            
            mapping.TryGetValue(columnName, out var result);
            return result;
        }

        /// <summary>
        /// Updates the stored column index when a matching header is discovered, marking the row as a header.
        /// </summary>
        /// <param name="oldVal">The previously stored column index.</param>
        /// <param name="range">The header dictionary to search.</param>
        /// <param name="columnName">The logical column name.</param>
        /// <param name="isHeader">Reference flag indicating whether the current row is a header.</param>
        /// <returns>The updated column index.</returns>
        public static int GetColumnFromRange(this int oldVal, Dictionary<string, int>? range, string columnName, ref bool isHeader)
        {
            var value = range.GetColumnFromRange(columnName);
            if (value <= 0) return oldVal;
            
            isHeader = true;
            return value;
        }
    }
}
