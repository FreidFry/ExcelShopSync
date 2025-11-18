using OfficeOpenXml;

namespace ExcelShSy.Core.Interfaces.Excel
{
    /// <summary>
    /// Represents an Excel file used during synchronization, including metadata and parsed sheets.
    /// </summary>
    public interface IExcelFile
    {
        /// <summary>
        /// Gets or sets the full file system location of the Excel file.
        /// </summary>
        string FileLocation { get; set; }

        /// <summary>
        /// Gets or sets the name of the Excel file.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Gets or sets the EPPlus package that provides access to the workbook contents.
        /// </summary>
        ExcelPackage ExcelPackage { get; set; }

        /// <summary>
        /// Gets or sets the associated shop name for this file, if known.
        /// </summary>
        string? ShopName { get; set; }

        /// <summary>
        /// Gets or sets the list of sheet abstractions parsed from the workbook.
        /// </summary>
        List<IExcelSheet>? SheetList { get; set; }

        /// <summary>
        /// Displays detailed information about the Excel file to the user.
        /// </summary>
        Task ShowFileDetails();
    }
}
