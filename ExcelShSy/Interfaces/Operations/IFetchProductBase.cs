using ExcelShSy.Core.Interfaces.Excel;

namespace ExcelShSy.Core.Interfaces.Operations
{
    /// <summary>
    /// Provides an abstraction for reading product data from an Excel file.
    /// </summary>
    public interface IFetchProductBase
    {
        /// <summary>
        /// Reads all relevant product data from the provided Excel file.
        /// </summary>
        /// <param name="file">The Excel file that contains product data.</param>
        void FetchAllProducts(IExcelFile file);
    }
}