using Avalonia.Controls;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;
using static ExcelShSy.Core.WindowHelper;

namespace ExcelShSy.Infrastructure.Services.Storage
{
    public class FileProvider(IExcelFileFactory excelFileFactory) : IFileProvider
    {
        public List<IExcelFile> FetchExcelFile(List<string> paths)
        {
            List<IExcelFile> result = [];
            result.AddRange(paths.Select(excelFileFactory.Create));

            return result;
        }

        public IExcelFile FetchExcelFile(string path) => excelFileFactory.Create(path);
        

        public async Task<List<string>?> PickExcelFilePaths()
        {
            var fileDialog = new OpenFileDialog
            {
                AllowMultiple = true,
                Filters = [new FileDialogFilter { Name = "Excel Files", Extensions = { "xls", "xlsx", "xlsm" } }]
            };
            var mainWindow = GetActiveWindow();
            if (mainWindow == null)
            {
                await MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard("Error", "No found active window. \n[ExcelShSy.Infrastructure.Services.Storage.PickExcelFilePaths()]").ShowAsync();
                return null;
            }
            var fileNames = await fileDialog.ShowAsync(mainWindow);
            if (fileNames == null || fileNames.Length == 0)
                return null;

            return [..fileNames];
        }

        public async Task<string?> PickExcelFilePath()
        {
            var fileDialog = new OpenFileDialog
            {
                AllowMultiple = false,
                Filters = [new FileDialogFilter { Name = "Excel Files", Extensions = { "xls", "xlsx", "xlsm" } }]
            };
            var mainWindow = GetActiveWindow();
            if (mainWindow == null)
            {
                await MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard("Error", "No found active window. \n[ExcelShSy.Infrastructure.Services.Storage.PickExcelFilePaths()]").ShowAsync();
                return null;
            }
            
            var filename = await fileDialog.ShowAsync(mainWindow);
            if (filename == null || filename.Length == 0)
                return null;
            return filename[0];
        }
    }
}
