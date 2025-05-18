using ExcelShSy.Core.Interfaces;
using ExcelShSy.Infrastracture.Persistance.Helpers;
using OfficeOpenXml;
using System.Windows;

namespace ExcelShSy.Infrastracture.Persistance.Model
{
    public class ExcelPage : IExcelPage
    {
        
        public string PageName { get; set; }
        public ExcelWorksheet ExcelWorksheet { get; set; }
        public List<string> UndefindedHeaders { get; set; }
        public Dictionary<string, int> Headers { get; set; } = [];

        public ExcelPage(ExcelWorksheet worksheet)
        {
            PageName = worksheet.Name;
            ExcelWorksheet = worksheet;
            UndefindedHeaders = GetHeaders(worksheet);
            ShowInfo();
        }

        public void ShowInfo()
        {
            if (Headers != null)
                MessageBox.Show($"{PageName}\n\n{string.Join("\n", Headers.Select(kv => $"{kv.Key}: {kv.Value}"))}");
            else
                MessageBox.Show($"{PageName}\n\nHeaders is null.");

            if (UndefindedHeaders != null)
                MessageBox.Show($"{PageName}\n\n{string.Join("\n", UndefindedHeaders)}");
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

            return AssistanceMethods.GetRowValues(worksheet, firstRow, firstColumn, lastColumn);
        }
    }
}
