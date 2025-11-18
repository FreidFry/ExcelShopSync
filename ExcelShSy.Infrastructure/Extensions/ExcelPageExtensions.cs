using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Infrastructure.Services.Common;
using OfficeOpenXml;

namespace ExcelShSy.Infrastructure.Extensions
{
    /// <summary>
    /// Provides helper methods for analyzing Excel sheets and worksheets.
    /// </summary>
    public static class ExcelPageExtensions
    {
        /// <summary>
        /// Attempts to identify the shop associated with the given sheet by comparing headers to stored templates.
        /// </summary>
        /// <param name="page">The sheet to analyze.</param>
        /// <param name="shopStorage">The shop storage used for comparison.</param>
        /// <returns>The inferred shop name or an empty string if unknown.</returns>
        public static string GetShop(this IExcelSheet? page, IShopStorage shopStorage)
        {
            var headerCount = page?.UnmappedHeaders?.Count;
            if (headerCount is null or 0) return string.Empty;

            Dictionary<string, int> shopScore = [];
            var shopKeys = shopStorage.GetShopList();

            foreach (var shopKey in shopKeys)
            {
                var shopColumn = shopStorage.GetShopMapping(shopKey)?.UnmappedHeaders;
                if (shopColumn == null || shopColumn.Count == 0) continue;
                var score = page?.UnmappedHeaders?.Keys.Intersect(shopColumn).Count();
                shopScore.Add(shopKey, score ?? 0);
                if (score >= headerCount / 2 && headerCount > 10) return shopKey;
            }
            if (shopScore.Count == 0) return string.Empty;
            var shopName = shopScore.OrderByDescending(x => x.Value).FirstOrDefault().Key;
            shopScore.TryGetValue(shopName, out var totalScore);
            return totalScore > 0 ? shopName : string.Empty;
        }

        /// <summary>
        /// Detects the likely language used in the sheet headers.
        /// </summary>
        /// <param name="page">The sheet to analyze.</param>
        /// <returns>The language identifier.</returns>
        public static string GetLanguage(this IExcelSheet? page)
        {
            if (page?.UnmappedHeaders == null || page.UnmappedHeaders.Count == 0)
                return "unknown";

            var allText = string.Join(" ", page.UnmappedHeaders.Keys.Take(15).ToList());
            var detector = new LanguageIdentifier();
            return detector.IdentifyLanguage(allText);
        }

        /// <summary>
        /// Enumerates row indexes excluding the first row (typically containing headers).
        /// </summary>
        /// <param name="worksheet">The worksheet to inspect.</param>
        /// <returns>A sequence of row indexes.</returns>
        public static IEnumerable<int> GetFullRowRangeWithoutFirstRow(this ExcelWorksheet? worksheet)
        {
            if (worksheet == null) return [];
            var start = worksheet.Dimension.Start.Row + 1;
            var end = worksheet.Dimension.End.Row;
            var rows = end - start + 1;

            return Enumerable.Range(start, rows);
        }

        /// <summary>
        /// Enumerates all row indexes in the worksheet.
        /// </summary>
        /// <param name="worksheet">The worksheet to inspect.</param>
        /// <returns>A sequence of row indexes.</returns>
        public static IEnumerable<int> GetFullRowRange(this ExcelWorksheet? worksheet)
        {
            if (worksheet == null) return [];
            var start = worksheet.Dimension.Start.Row;
            var end = worksheet.Dimension.End.Row;
            var rows = end - start + 1;

            return Enumerable.Range(start, rows);
        }

        /// <summary>
        /// Builds a mapping of header text to column indexes for a given row.
        /// </summary>
        /// <param name="worksheet">The worksheet to inspect.</param>
        /// <param name="fromRow">The row index to read.</param>
        /// <returns>A dictionary of header text to column index, or <c>null</c> if the row is empty.</returns>
        public static Dictionary<string, int>? GetRowValueColumnMap(this ExcelWorksheet worksheet, int fromRow)
        {
            var toColumn = worksheet.Dimension.End.Column;
            var range = worksheet.Cells[fromRow, 1, fromRow, toColumn];
            var hasEmpty = range.Any(cell => cell.Value != null);

            if (!hasEmpty) return null;
            return range.Where(cell => !string.IsNullOrWhiteSpace(cell.Value?.ToString()))
                .GroupBy(cell => cell.Value.ToString())
                .ToDictionary(g => g.Key, g => g.First().Start.Column)!;
        }
    }
}
