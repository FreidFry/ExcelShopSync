using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;
using Microsoft.Win32;

namespace ExcelShSy.Core.Services
{
    public class FileProvider : IFileProvider
    {
        private readonly IServiceProvider _provider;
        private readonly IExcelFileFactory _excelFileFactory;

        public FileProvider(IServiceProvider provider, IExcelFileFactory excelFileFactory)
        {
            _provider = provider;
            _excelFileFactory = excelFileFactory;
        }

        public List<IExcelFile> GetFiles(List<string> paths)
        {
            var result = new List<IExcelFile>();

            foreach (var path in paths)
            {
                var instance = _excelFileFactory.Create(path);
                //var instance = ActivatorUtilities.CreateInstance(_provider, typeof(ExcelFile), path) as IExcelFile;
                if (instance != null)
                    result.Add(instance);
            }

            return result;
        }

        public List<string> GetPaths()
        {
            OpenFileDialog fileDialog = new()
            {
                Filter = "Excel Files|*.xls;*.xlsx;*.xlsm",
                Multiselect = true
            };

            if (fileDialog.ShowDialog() == false || fileDialog.FileNames.Length == 0)
                return [];

            var result = new List<string>();

            foreach (var fileName in fileDialog.FileNames)
            {
                result.Add(fileName);
            }

            return result;
        }
    }
}
