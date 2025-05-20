using ExcelShSy.Core.Interfaces;
using ExcelShSy.Core.Services;
using ExcelShSy.Infrastracture.Persistance.Helpers;
using ExcelShSy.Infrastracture.Persistance.ShopData;
using OfficeOpenXml;
using System.Windows;

namespace ExcelShSy.Infrastracture.Persistance.Model
{
    public class ExcelPage : IExcelPage
    {
        
        public string PageName { get; set; }
        public ExcelWorksheet ExcelWorksheet { get; set; }
        public Dictionary<string, int>? UndefinedHeaders { get; set; }
        public Dictionary<string, int>? Headers { get; set; }

        public ExcelPage(ExcelWorksheet worksheet)
        {
            PageName = worksheet.Name;
            ExcelWorksheet = worksheet;
            UndefinedHeaders = GetHeaders(worksheet);
            Headers = GetRealHeaders();
        }

        public bool ShowInfo()
        {
            string response;
            if (Headers != null)
                response = $"{PageName}\n\n{string.Join("\n", Headers.Select(kv => $"{kv.Key}: {kv.Value}"))}";
            else
                response = $"{PageName}\n\nHeaders is null.";
            var s = MessageBox.Show(response, "kek", MessageBoxButton.OKCancel);
            if (s == MessageBoxResult.OK) return true; //show next message

            return false; //stop show message
        }

        Dictionary<string, int> GetHeaders(ExcelWorksheet worksheet)
        {
            if(worksheet == null || worksheet.Dimension == null)
                return [];

            var dimension = worksheet.Dimension;

            var firstColumn = dimension.Start.Column;
            var firstRow = dimension.Start.Row;
            var lastColumn = dimension.End.Column;

            return AssistanceMethods.GetRowValues(worksheet, firstRow, lastColumn);
        }

        public string GetShop()
        {
            var headerCount = UndefinedHeaders.Count;
            if (headerCount == 0) return ShopNameConstant.Unknown;

            var ShopData = new ShopMappings().Shops;

            Dictionary<string, int> shopScore = [];
            var ShopKeys = ShopData.Keys;

            foreach (var shopKey in ShopKeys)
            {
                var shopColumn = ShopData[shopKey].columns;

                var score = UndefinedHeaders.Keys.Intersect(shopColumn).Count();
                shopScore.Add(shopKey, score);
                if (score >= headerCount / 2 && headerCount > 10) return shopKey;
            }

            var shopName = shopScore.OrderByDescending(x => x.Value).First().Key;
            return shopScore[shopName] > 0 ? shopName : ShopNameConstant.Unknown;
        }

        public string GetLanguague()
        {
            if (UndefinedHeaders == null || UndefinedHeaders.Count == 0)
                return "unknown";

            var allText = string.Join(" ", UndefinedHeaders.Keys.Take(15).ToList());
            var detector = new LanguageDetector();
            return detector.Detect(allText);
        }

        Dictionary<string, int>? GetRealHeaders()
        {
            var template = ColumnMapping.Columns;

            return UndefinedHeaders == null ? [] : template
            .SelectMany(pair => pair.Value, (pair, name) => new { Key = pair.Key, Name = name })
            .Where(x => UndefinedHeaders.ContainsKey(x.Name))
            .GroupBy(x => x.Key)
            .ToDictionary(
                g => g.Key,
                g => UndefinedHeaders[g.First().Name]
            );
        }
    }
}
