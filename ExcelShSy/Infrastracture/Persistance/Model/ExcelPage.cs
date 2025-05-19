using ExcelShSy.Core.Interfaces;
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
        public List<string> UndefinedHeaders { get; set; }
        public Dictionary<string, int> Headers { get; set; } = [];

        public ExcelPage(ExcelWorksheet worksheet)
        {
            PageName = worksheet.Name;
            ExcelWorksheet = worksheet;
            UndefinedHeaders = GetHeaders(worksheet);
            //ShowInfo();
        }

        public void ShowInfo()
        {
            if (Headers != null)
                MessageBox.Show($"{PageName}\n\n{string.Join("\n", Headers.Select(kv => $"{kv.Key}: {kv.Value}"))}");
            else
                MessageBox.Show($"{PageName}\n\nHeaders is null.");

            if (UndefinedHeaders != null)
                MessageBox.Show($"{PageName}\n\n{string.Join("\n", UndefinedHeaders)}");
            else
                MessageBox.Show($"{PageName}\n\nHeaders is null.");
        }

        List<string> GetHeaders(ExcelWorksheet worksheet)
        {
            if(worksheet == null || worksheet.Dimension == null)
                return [];

            var dimension = worksheet.Dimension;

            var firstColumn = dimension.Start.Column;
            var firstRow = dimension.Start.Row;
            var lastColumn = dimension.End.Column;

            var columns = AssistanceMethods.GetRowValues(worksheet, firstRow, firstColumn, lastColumn);
            return columns.Distinct().ToList();
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

                var score = UndefinedHeaders.Intersect(shopColumn).Count();
                shopScore.Add(shopKey, score);
                if (score >= headerCount / 2 && headerCount > 10) return shopKey;
            }

            var shopName = shopScore.OrderByDescending(x => x.Value).First().Key;
            return shopScore[shopName] > 0 ? shopName : ShopNameConstant.Unknown;
        }
    }
}
