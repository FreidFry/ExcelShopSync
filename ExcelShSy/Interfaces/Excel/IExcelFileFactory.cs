namespace ExcelShSy.Core.Interfaces.Excel
{
    /// <summary>
    /// Factory for creating Excel file abstractions from file system paths.
    /// </summary>
    public interface IExcelFileFactory
    {
        /// <summary>
        /// Creates an <see cref="IExcelFile"/> for the provided file path.
        /// </summary>
        /// <param name="path">The path to the Excel file.</param>
        /// <returns>The wrapped Excel file.</returns>
        IExcelFile Create(string path);
    }
}
