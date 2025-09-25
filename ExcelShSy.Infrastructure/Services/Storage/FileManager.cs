using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Extensions;

using static ExcelShSy.Infrastructure.Extensions.FileNameExtensions;

namespace ExcelShSy.Infrastructure.Services.Storage
{
    public class FileManager : IFileManager
    {
        private readonly IFileProvider _fileProvider;
        private readonly IFileStorage _fileStorage;
        private readonly IProductStorage _dataProduct;
        private readonly ILogger _logger;

        private readonly IGetProductManager _getProductManager;

        public List<string> TargetPaths { get; set; } = [];
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

        public IExcelFile GetFileDetails(string path)
        {
            return _fileProvider.FetchExcelFile([path])[0];
        }

        public void InitializeTargetFiles()
        {
            var files = _fileProvider.FetchExcelFile(TargetPaths);
            if (files != null)
                _fileStorage.AddTarget(files);
        }

        public void InitializeSourceFiles()
        {
            var files = _fileProvider.FetchExcelFile(SourcePaths);
            if (files != null)
                _fileStorage.AddSource(files);
        }

        public void InitializeAllFiles()
        {
            InitializeTargetFiles();
            InitializeSourceFiles();
            _getProductManager.FetchAllProducts();
        }

        public void ClearAfterComplete()
        {
            _dataProduct.ClearData();
            _fileStorage.ClearAllFiles();

            _logger.LogInfo("All files cleared");
        }

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

        public void RemoveSourcePath(string path)
        {
            if (SourcePaths.Contains(path))
            {
                _logger.LogInfo($"Remove source files path: {Path.GetFileNameWithoutExtension(path)}");
                SourcePaths.Remove(path);
            }
        }

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