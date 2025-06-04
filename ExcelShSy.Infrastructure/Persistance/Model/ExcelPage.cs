using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Infrastructure.Extensions;

using OfficeOpenXml;

namespace ExcelShSy.Infrastructure.Persistance.Model
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

        public string ShowInfo()
        {
            string response;
            if (!Headers.IsNullOrEmpty())
                response = $"{PageName}\n\n{string.Join("\n", Headers.Select(kv => $"{kv.Key}: {kv.Value}"))}";
            else
                response = $"{PageName}\n\nHeaders is null.";
            return response;
        }
    }
}
