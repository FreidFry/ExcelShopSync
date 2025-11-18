using OfficeOpenXml;

namespace ExcelShSy.Core.Interfaces.Excel
{
    /// <summary>
    /// Factory for wrapping EPPlus worksheets with the application-specific sheet abstraction.
    /// </summary>
    public interface IExcelPageFactory
    {
        /// <summary>
        /// Creates an <see cref="IExcelSheet"/> representation for the specified worksheet.
        /// </summary>
        /// <param name="worksheet">The worksheet to wrap.</param>
        /// <returns>A new sheet abstraction for the worksheet.</returns>
        IExcelSheet Create(ExcelWorksheet worksheet);
    }
}
