using ExcelShSy.Core.Interfaces.Excel;

namespace ExcelShSy.Core.Interfaces.Storage
{
    /// <summary>
    /// Describes operations for locating, loading, and selecting Excel files for processing.
    /// </summary>
    public interface IFileProvider
    {
        /// <summary>
        /// Loads Excel files from the specified file paths.
        /// </summary>
        /// <param name="paths">A collection of file paths pointing to Excel documents.</param>
        /// <returns>A list of parsed Excel file representations.</returns>
        List<IExcelFile> FetchExcelFile(List<string> paths);

        /// <summary>
        /// Loads a single Excel file from the specified path.
        /// </summary>
        /// <param name="paths">The file path pointing to an Excel document.</param>
        /// <returns>The parsed Excel file representation.</returns>
        IExcelFile FetchExcelFile(string paths);

        /// <summary>
        /// Prompts the user to select multiple Excel files and returns the chosen paths.
        /// </summary>
        /// <returns>The list of selected file paths, or <c>null</c> if the selection was cancelled.</returns>
        Task<List<string>?> PickExcelFilePaths();

        /// <summary>
        /// Prompts the user to select a single Excel file and returns the chosen path.
        /// </summary>
        /// <returns>The selected file path, or <c>null</c> if the selection was cancelled.</returns>
        Task<string?> PickExcelFilePath();
    }
}
