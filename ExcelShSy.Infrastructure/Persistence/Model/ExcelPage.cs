using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Infrastructure.Extensions;
using OfficeOpenXml;

namespace ExcelShSy.Infrastructure.Persistence.Model
{
    /// <summary>
    /// Represents an Excel worksheet along with header metadata.
    /// </summary>
    public class ExcelPage : IExcelSheet
    {
        
        /// <inheritdoc />
        public string SheetName { get; set; }
        /// <inheritdoc />
        public ExcelWorksheet Worksheet { get; set; }
        /// <inheritdoc />
        public Dictionary<string, int>? UnmappedHeaders { get; set; }
        /// <inheritdoc />
        public Dictionary<string, int>? MappedHeaders { get; set; }

        public ExcelPage(ExcelWorksheet worksheet)
        {
            SheetName = worksheet.Name;
            Worksheet = worksheet;
        }

        /// <inheritdoc />
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
