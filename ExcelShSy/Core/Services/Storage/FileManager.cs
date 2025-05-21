using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
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
        private readonly IGetPricesFromSource _getPricesFromSource;
        private readonly IGetProductFromPrice _getProductFromPrice;

        public List<string> TargetPath { get; set; } = [];
        public List<string> SourcePath { get; set; } = [];

        public FileManager(IFileProvider fileProvider,
            IFileStorage fileStorage,
            IDataProduct dataProduct,
            IGetPricesFromSource getPricesFromSource,
            IGetProductFromPrice getProductFromPrice)
        {
            _fileProvider = fileProvider;
            _fileStorage = fileStorage;
            _dataProduct = dataProduct;

            _getPricesFromSource = getPricesFromSource;
            _getProductFromPrice = getProductFromPrice;
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

        public void InitializeFiles(bool? isPrice)
        {
            _dataProduct.ClearAll();
            _fileStorage.ClearAll();

            AddTargetFiles();
            AddSourceFiles();

            if (isPrice == true)
                _getProductFromPrice.GetProducts();
            else _getPricesFromSource.GetAllPrice();
        }

        public void AddSourceFilesPath(Label label)
        {
            var paths = _fileProvider.GetPaths();

            string LastPath = string.Empty;

            foreach (var path in paths)
                if (File.Exists(path))
                {
                    SourcePath.Add(path);
                    LastPath = path;
                }
            SetLastPath(label, LastPath);
        }

        public void AddTargetFilesPath(Label label)
        {
            var paths = _fileProvider.GetPaths();

            string LastPath = string.Empty;

            foreach (var path in paths)
                if (File.Exists(path))
                {
                    TargetPath.Add(path);
                    LastPath = path;
                }
            SetLastPath(label, LastPath);
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

        private static void SetLastPath(Label lable, string path)
        {
            lable.Content = Path.GetFileName(path);
        }
    }
}
