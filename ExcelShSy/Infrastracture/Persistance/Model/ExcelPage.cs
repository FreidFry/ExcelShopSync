using ExcelShSy.Infrastracture.Persistance.Interfaces;
using OfficeOpenXml;
using System.Windows;

namespace ExcelShSy.Infrastracture.Persistance.Model
{
    public class ExcelPage : IExcelPage
    {

        public string PageName { get; set; }
        public ExcelWorksheet ExcelWorksheet { get; set; }
        public Dictionary<string, int> Headers { get; set; } = [];

        public ExcelPage(ExcelWorksheet worksheet)
        {
            PageName = worksheet.Name;
            ExcelWorksheet = worksheet;
        }

        public void ShowInfo()
        {
            if (Headers != null)
                MessageBox.Show($"{PageName}\n\n{string.Join("\n", Headers.Select(kv => $"{kv.Key}: {kv.Value}"))}");
            else
                MessageBox.Show($"{PageName}\n\nHeaders is null.");
        }
    }
}
