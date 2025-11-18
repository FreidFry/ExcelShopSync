using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;

namespace ExcelShSy.Infrastructure.Services.Storage
{
    /// <summary>
    /// In-memory storage for source and target Excel files used during synchronization.
    /// </summary>
    public class FileStorage : IFileStorage
    {
        /// <inheritdoc />
        public List<IExcelFile> Target { get; } = [];
        /// <inheritdoc />
        public List<IExcelFile> Source { get; } = [];



        /// <inheritdoc />
        public void AddTarget(List<IExcelFile> files)
        {
            Target.AddRange(files);
        }
        /// <inheritdoc />
        public void AddSource(List<IExcelFile> files)
        {
            Source.AddRange(files);
        }

        /// <inheritdoc />
        public void ClearTargetFiles() => Target.Clear();
        /// <inheritdoc />
        public void ClearSourceFiles() => Source.Clear();

        /// <inheritdoc />
        public void ClearAllFiles()
        {
            Target.Clear();
            Source.Clear();
        }
    }
}
