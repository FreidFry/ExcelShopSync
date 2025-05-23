using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Services.Common;
using ExcelShSy.Infrastracture.Persistance.Model;
using ExcelShSy.Infrastracture.Persistance.ShopData;

using OfficeOpenXml;

namespace ExcelShSy.Core.Extensions
{
    public static class ExcelPageExtensions
    {
        public static string GetShop(this IExcelPage? page)
        {
            var headerCount = page?.UndefinedHeaders?.Count;
            if (headerCount == null || headerCount == 0) return ShopNameConstant.Unknown;

            var ShopData = new ShopMappings().Shops;

            Dictionary<string, int> shopScore = [];
            var ShopKeys = ShopData.Keys;

            foreach (var shopKey in ShopKeys)
            {
                var shopColumn = ShopData[shopKey].columns;

                var score = page?.UndefinedHeaders?.Keys.Intersect(shopColumn).Count();
                shopScore.Add(shopKey, score ?? 0);
                if (score >= headerCount / 2 && headerCount > 10) return shopKey;
            }

            var shopName = shopScore.OrderByDescending(x => x.Value).First().Key;
            return shopScore[shopName] > 0 ? shopName : ShopNameConstant.Unknown;
        }

        public static string GetLanguague(this IExcelPage? page)
        {
            if (page?.UndefinedHeaders == null || page.UndefinedHeaders.Count == 0)
                return "unknown";

            var allText = string.Join(" ", page.UndefinedHeaders.Keys.Take(15).ToList());
            var detector = new LanguageDetector();
            return detector.Detect(allText);
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

#pragma warning disable CS8714, CS8621, CS8619
            if (HasEmpty)
            {
                var RowValues = range.Where(cell => !string.IsNullOrWhiteSpace(cell.Value?.ToString()))
                    .GroupBy(cell => cell.Value.ToString())
                    .ToDictionary(g => g.Key, g => g.First().Start.Column);
                return RowValues;
            }
#pragma warning restore CS8714, CS8621, CS8619

            return null;
        }
    }
}
