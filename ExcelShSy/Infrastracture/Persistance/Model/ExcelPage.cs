using ExcelShSy.Core.Extensions;
using ExcelShSy.Core.Interfaces.Excel;
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
        }

        public bool ShowInfo()
        {
            string response;
            if (!Headers.IsNullOrEmpty())
                response = $"{PageName}\n\n{string.Join("\n", Headers.Select(kv => $"{kv.Key}: {kv.Value}"))}";
            else
                response = $"{PageName}\n\nHeaders is null.";
            var s = MessageBox.Show(response, "kek", MessageBoxButton.OKCancel);
            if (s == MessageBoxResult.OK) return true; //show next message

            return false; //stop show message
        }
    }
}
