using ExcelShopSync.Properties;
using OfficeOpenXml;
using System.Windows;

namespace ExcelShopSync.Modules
{
    class PageBase
    {
        public string PageName { get; }
        public ExcelWorksheet ExcelWorksheet { get; }
        public Dictionary<string, int>? Headers { get; set; }

        public PageBase(ExcelWorksheet worksheet)
        {
            ExcelWorksheet = worksheet;
            PageName = worksheet.Name;
            Headers = GetHeaders(worksheet);
        }

        private Dictionary<string, int>? GetHeaders(ExcelWorksheet worksheet)
        {
            if (worksheet == null || worksheet.Dimension == null)
            {
                MessageBox.Show("Worksheet is null");
                return new Dictionary<string, int> { { string.Empty, 1 } };
            }
            var ShopTemplate = new ShopBase().Columns;
            Dictionary<string, int> headers = [];

            int row = worksheet.Dimension.Start.Row;
            for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
            {
                string? head = worksheet.Cells[row, col].Value?.ToString();
                if (head != null)
                {
                    string key = ShopTemplate.FirstOrDefault(kv => kv.Value.Contains(head)).Key;
                    if (!string.IsNullOrEmpty(key)) headers.Add(key, col);
                }
            }
            MessageBox.Show(string.Join(", ", headers.Select(kv => $"{kv.Key}: {kv.Value}")));
            return headers.Count > 0 ? headers : null;
        }
    }
}
