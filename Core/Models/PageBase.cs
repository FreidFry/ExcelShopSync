using static ExcelShopSync.Core.Static.Shops;
using OfficeOpenXml;
using System.Windows;
using ExcelShopSync.Core.Static;

namespace ExcelShopSync.Core.Models
{
    public class PageBase : IPageBase
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
            Dictionary<string, int> headers = [];

            int row = worksheet.Dimension.Start.Row;
            for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
            {
                string? head = worksheet.Cells[row, col].Value?.ToString();
                if (head != null)
                {
                    string key = ShopBase.Columns.FirstOrDefault(kv => kv.Value.Contains(head)).Key;
                    if (!string.IsNullOrEmpty(key)) headers.Add(key, col);
                }
            }
            return headers.Count > 0 ? headers : null;
        }
    }
}
