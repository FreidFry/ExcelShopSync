using OfficeOpenXml;

namespace ExcelShSy.Core.Interfaces.Excel
{
    /// <summary>
    /// Represents a worksheet within an Excel workbook, including header mapping metadata.
    /// </summary>
    public interface IExcelSheet
    {
        /// <summary>
        /// Gets or sets the name of the worksheet.
        /// </summary>
        string SheetName { get; set; }

        /// <summary>
        /// Gets or sets the underlying EPPlus worksheet instance.
        /// </summary>
        ExcelWorksheet Worksheet { get; set; }

        /// <summary>
        /// Gets or sets unmapped headers with their column indexes.
        /// </summary>
        Dictionary<string, int>? UnmappedHeaders { get; set; }

        /// <summary>
        /// Gets or sets mapped headers with their column indexes.
        /// </summary>
        Dictionary<string, int>? MappedHeaders { get; set; }

        /// <summary>
        /// Returns a human-readable description of the worksheet and its mapped columns.
        /// </summary>
        /// <returns>A summary string describing the worksheet.</returns>
        string ShowPageDetails();

    }
}
