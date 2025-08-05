using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Infrastructure.Extensions;

using OfficeOpenXml;

namespace ExcelShSy.Infrastructure.Persistance.Model
{
    public class ExcelPage : IExcelSheet
    {
        
        public string SheetName { get; set; }
        public ExcelWorksheet Worksheet { get; set; }
        public Dictionary<string, int>? UnmappedHeaders { get; set; }
        public Dictionary<string, int>? MappedHeaders { get; set; }

        public ExcelPage(ExcelWorksheet worksheet)
        {
            SheetName = worksheet.Name;
            Worksheet = worksheet;
        }

        public string ShowPageDetails()
        {
            string response;
            if (!MappedHeaders.IsNullOrEmpty())
                response = $"{SheetName}\n\n{string.Join("\n", MappedHeaders.Select(kv => $"{kv.Key}: {kv.Value}"))}";
            else
                response = $"{SheetName}\n\nHeaders is null.";
            return response;
        }
    }
}
