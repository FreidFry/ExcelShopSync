using OfficeOpenXml;
using ExcelShopSync.Modules;
using ExcelShopSync.Properties;
using System.Windows;

namespace ExcelShopSync.Services.Price
{
    class PriceWithPriceList
    {
        Dictionary<string, decimal> products = [];
        public PriceWithPriceList(List<Target> targets)
        {
            var shop = new ShopBase().Columns;
            foreach (var target in targets)
                foreach (var page in target.Pages)
                {
                    ExcelWorksheet worksheet = page.ExcelWorksheet;
                    Dictionary<string, int> headers = new(4);

                    for (int row = worksheet.Dimension.Start.Row + 1; row <= worksheet.Dimension.End.Row; row++)
                    {
                        string? article, articleComplect;
                        string? price, priceComplect;

                        bool isHeaderRow = false;
                        for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                        {
                            string? cell = worksheet.Cells[row, col].FirstOrDefault()?.Value.ToString();
                            if (cell == null) continue;

                            if (shop.Any(kvp => kvp.Value.Contains(cell)))
                            {
                                isHeaderRow = true;

                                string key = shop.FirstOrDefault(kv => kv.Value.Contains(cell)).Key;
                                if (!string.IsNullOrEmpty(key)) headers.Add(key, col);
                            }
                            if (isHeaderRow) continue;
                        }
                        isHeaderRow = false;
                    }
                    MessageBox.Show(string.Join(", ", headers.Select(kv => $"{kv.Key}: {kv.Value}")));
                }
        }
    }
}
