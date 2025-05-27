using ExcelShSy.Core.Extensions;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;

using System.IO;
using System.Windows.Controls;

namespace ExcelShSy.Core.Services.Storage
{
    public class FileManager : IFileManager
    {
        private readonly IFileProvider _fileProvider;
        private readonly IFileStorage _fileStorage;
        private readonly IDataProduct _dataProduct;
        private readonly IGetProductManager _getProductManager;

        public List<string> TargetPath { get; set; } = [];
        public List<string> SourcePath { get; set; } = [];

        public FileManager(IFileProvider fileProvider,
            IFileStorage fileStorage,
            IDataProduct dataProduct,
            IGetProductManager getProductManager)
        {
            _fileProvider = fileProvider;
            _fileStorage = fileStorage;
            _dataProduct = dataProduct;

            _getProductManager = getProductManager;
        }

        public void AddTargetFiles()
        {
            var files = _fileProvider.GetFiles(TargetPath);
            if (files != null)
                _fileStorage.AddTarget(files);
        }

        public void AddSourceFiles()
        {
            var files = _fileProvider.GetFiles(SourcePath);
            if (files != null)
                _fileStorage.AddSource(files);
        }

        public void InitializeFiles()
        {
            _dataProduct.ClearAll();
            _fileStorage.ClearAll();

            AddTargetFiles();
            AddSourceFiles();
            _getProductManager.GetAllProduct();
        }

        public void AddSourceFilesPath(Label label)
        {
            var paths = _fileProvider.GetPaths();
            if (paths.IsNullOrEmpty()) return;

            SourcePath.AddRange(paths.Distinct().Except(SourcePath));

            SetLastPath(label, SourcePath.Last());
        }

        public void AddTargetFilesPath(Label label)
        {
            var paths = _fileProvider.GetPaths();
            if (paths.IsNullOrEmpty()) return;

            TargetPath.AddRange(paths.Distinct().Except(TargetPath));

            SetLastPath(label, TargetPath.Last());
        }

        public void RemoveSourceFilesPath(string path)
        {
            if (SourcePath.Contains(path))
                SourcePath.Remove(path);
        }

        public void RemoveTargetFilesPath(string path)
        {
            if (TargetPath.Contains(path))
                TargetPath.Remove(path);
        }

        static void SetLastPath(Label lable, string? path)
        {
            if (!string.IsNullOrEmpty(path))
                lable.Content = Path.GetFileName(path);
        }
    }
}
