using Avalonia.Controls;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;
using static ExcelShSy.Core.WindowHelper;

namespace ExcelShSy.Infrastructure.Services.Storage
{
    public class FileProvider : IFileProvider
    {
        private readonly IExcelFileFactory _excelFileFactory;
        
        public FileProvider(IExcelFileFactory excelFileFactory)
        {
            _excelFileFactory = excelFileFactory;
        }

        public List<IExcelFile> FetchExcelFile(List<string> paths)
        {
            List<IExcelFile> result = [];

            foreach (var path in paths)
            {
                var instance = _excelFileFactory.Create(path);
                if (instance != null)
                    result.Add(instance);
            }

            return result;
        }

        [Obsolete("Obsolete")]
        public async Task<List<string>?> PickExcelFilePaths()
        {
            var fileDialog = new OpenFileDialog
            {
                AllowMultiple = true,
                Filters = [new FileDialogFilter { Name = "Excel Files", Extensions = { "xls", "xlsx", "xlsm" } }]
            };

            var fileNames = await fileDialog.ShowAsync(GetActiveWindow());
            if (fileNames == null || fileNames.Length == 0)
                return null;

            return [..fileNames];

        }
    }
}
