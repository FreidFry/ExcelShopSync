using ExcelShSy.Core.Interfaces.Excel;

namespace ExcelShSy.Core.Interfaces.Storage
{
    /// <summary>
    /// Provides an abstraction for storing source and target Excel files used during synchronization.
    /// </summary>
    public interface IFileStorage
    {
        /// <summary>
        /// Gets the collection of target Excel files that will receive data.
        /// </summary>
        List<IExcelFile> Target { get; }

        /// <summary>
        /// Gets the collection of source Excel files that provide data.
        /// </summary>
        List<IExcelFile> Source { get; }

        /// <summary>
        /// Adds the provided Excel files to the target collection.
        /// </summary>
        /// <param name="file">The files to append to the target list.</param>
        void AddTarget(List<IExcelFile> file);

        /// <summary>
        /// Adds the provided Excel files to the source collection.
        /// </summary>
        /// <param name="file">The files to append to the source list.</param>
        void AddSource(List<IExcelFile> file);

        /// <summary>
        /// Removes all target files from storage.
        /// </summary>
        void ClearTargetFiles();

        /// <summary>
        /// Removes all source files from storage.
        /// </summary>
        void ClearSourceFiles();

        /// <summary>
        /// Removes both source and target files from storage.
        /// </summary>
        void ClearAllFiles();
    }
}
