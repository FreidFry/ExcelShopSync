using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Extensions;

using static ExcelShSy.Infrastructure.Extensions.FileNameExtensions;

namespace ExcelShSy.Infrastructure.Services.Storage
{
    /// <summary>
    /// Coordinates file selection, loading, and cleanup for synchronization operations.
    /// </summary>
    public class FileManager : IFileManager
    {
        private readonly IFileProvider _fileProvider;
        private readonly IFileStorage _fileStorage;
        private readonly IProductStorage _dataProduct;
        private readonly ILogger _logger;

        private readonly IGetProductManager _getProductManager;

        /// <inheritdoc />
        public List<string> TargetPaths { get; set; } = [];
        /// <inheritdoc />
        public List<string> SourcePaths { get; set; } = [];

        public FileManager(IFileProvider fileProvider,
            IFileStorage fileStorage,
            IProductStorage dataProduct,
            ILogger logger,
            IGetProductManager getProductManager
            )
        {
            _fileProvider = fileProvider;
            _fileStorage = fileStorage;
            _dataProduct = dataProduct;
            _logger = logger;

            _getProductManager = getProductManager;
        }

        /// <summary>
        /// Retrieves file details for the provided path.
        /// </summary>
        /// <param name="path">The Excel file path.</param>
        /// <returns>The file abstraction.</returns>
        public IExcelFile GetFileDetails(string path)
        {
            return _fileProvider.FetchExcelFile([path])[0];
        }

        /// <summary>
        /// Loads target files into the shared file storage.
        /// </summary>
        public void InitializeTargetFiles()
        {
            var files = _fileProvider.FetchExcelFile(TargetPaths);
            _fileStorage.AddTarget(files);
        }

        /// <summary>
        /// Loads source files into the shared file storage.
        /// </summary>
        public void InitializeSourceFiles()
        {
            var files = _fileProvider.FetchExcelFile(SourcePaths);
            _fileStorage.AddSource(files);
        }

        /// <inheritdoc />
        public void InitializeAllFiles()
        {
            InitializeTargetFiles();
            InitializeSourceFiles();
            _getProductManager.FetchAllProducts();
        }

        /// <inheritdoc />
        public void ClearAfterComplete()
        {
            _dataProduct.ClearData();
            _fileStorage.ClearAllFiles();

            _logger.LogInfo("All files cleared");
        }

        /// <inheritdoc />
        public async void AddSourcePath()
        {
            var paths = await _fileProvider.PickExcelFilePaths();
            if (paths.IsNullOrEmpty())
            {
                _logger.LogInfo("Sources file empty");
                return;
            }

            SourcePaths.AddRange(paths.Distinct().Except(SourcePaths));

            SetLastPath("SourceLb", SourcePaths);
        }

        /// <inheritdoc />
        public async void AddTargetPath()
        {
            var paths = await _fileProvider.PickExcelFilePaths();
            if (paths.IsNullOrEmpty())
            {
                _logger.LogError("Targets file empty");
                return;
            }
            TargetPaths.AddRange(paths.Distinct().Except(TargetPaths));

            SetLastPath("TargetLb", TargetPaths);
        }

        /// <summary>
        /// Removes the specified source path and logs the action.
        /// </summary>
        /// <param name="path">The source file path to remove.</param>
        public void RemoveSourcePath(string path)
        {
            if (SourcePaths.Contains(path))
            {
                _logger.LogInfo($"Remove source files path: {Path.GetFileNameWithoutExtension(path)}");
                SourcePaths.Remove(path);
            }
        }

        /// <summary>
        /// Removes the specified target path and logs the action.
        /// </summary>
        /// <param name="path">The target file path to remove.</param>
        public void RemoveTargetPath(string path)
        {
            if (TargetPaths.Contains(path))
            {
                _logger.LogInfo($"Remove target files path: {Path.GetFileNameWithoutExtension(path)}");
                TargetPaths.Remove(path);
            }
        }
    }
}