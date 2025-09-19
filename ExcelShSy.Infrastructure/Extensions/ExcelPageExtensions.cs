using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Infrastructure.Services.Common;
using OfficeOpenXml;

namespace ExcelShSy.Infrastructure.Extensions
{
    public static class ExcelPageExtensions
    {
        public static string GetShop(this IExcelSheet? page, IShopStorage shopStorage)
        {
            var headerCount = page?.UnmappedHeaders?.Count;
            if (headerCount == null || headerCount == 0) return string.Empty;

            Dictionary<string, int> shopScore = [];
            var ShopKeys = shopStorage.GetShopList();

            foreach (var shopKey in ShopKeys)
            {
                var shopColumn = shopStorage.GetShopMapping(shopKey)?.UnmappedHeaders;
                if (shopColumn == null || shopColumn.Count == 0) continue;
                var score = page?.UnmappedHeaders?.Keys.Intersect(shopColumn).Count();
                shopScore.Add(shopKey, score ?? 0);
                if (score >= headerCount / 2 && headerCount > 10) return shopKey;
            }

            var shopName = shopScore.OrderByDescending(x => x.Value).First().Key;
            return shopScore[shopName] > 0 ? shopName : string.Empty;
        }

        public static string GetLanguague(this IExcelSheet? page)
        {
            if (page?.UnmappedHeaders == null || page.UnmappedHeaders.Count == 0)
                return "unknown";

            var allText = string.Join(" ", page.UnmappedHeaders.Keys.Take(15).ToList());
            var detector = new LanguageIdentifier();
            return detector.IdentifyLanguage(allText);
        }

        public static IEnumerable<int> GetFullRowRangeWithoutFirstRow(this ExcelWorksheet? worksheet)
        {
            if (worksheet == null) return [];
            var start = worksheet.Dimension.Start.Row + 1;
            var end = worksheet.Dimension.End.Row;
            var rows = end - start + 1;

            return Enumerable.Range(start, rows);
        }

        public static IEnumerable<int> GetFullRowRange(this ExcelWorksheet? worksheet)
        {
            if (worksheet == null) return [];
            var start = worksheet.Dimension.Start.Row;
            var end = worksheet.Dimension.End.Row;
            var rows = end - start + 1;

            return Enumerable.Range(start, rows);
        }

        public static Dictionary<string, int>? GetRowValueColumnMap(this ExcelWorksheet worksheet, int fromRow)
        {
            var toColumn = worksheet.Dimension.End.Column;
            var range = worksheet.Cells[fromRow, 1, fromRow, toColumn];
            bool HasEmpty = range.Any(cell => cell.Value != null);

            if (HasEmpty)
            {
                var RowValues = range.Where(cell => !string.IsNullOrWhiteSpace(cell.Value?.ToString()))
                    .GroupBy(cell => cell.Value.ToString())
                    .ToDictionary(g => g.Key, g => g.First().Start.Column);
                return RowValues;
            }

            return null;
        }
    }
}
