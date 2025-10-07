using OfficeOpenXml;

namespace ExcelShSy.Core.Interfaces.Excel
{
    public interface IExcelSheet
    {
        string SheetName { get; set; }
        ExcelWorksheet Worksheet { get; set; }

        Dictionary<string, int>? UnmappedHeaders { get; set; }
        Dictionary<string, int>? MappedHeaders { get; set; }

        string ShowPageDetails();

    }
}
