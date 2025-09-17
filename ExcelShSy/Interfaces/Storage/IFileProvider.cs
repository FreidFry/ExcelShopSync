using Avalonia.Controls;
using ExcelShSy.Core.Interfaces.Excel;

namespace ExcelShSy.Core.Interfaces.Storage
{
    public interface IFileProvider
    {
        List<IExcelFile> FetchExcelFile(List<string> paths);
        Task<List<string>?> PickExcelFilePaths();
    }
}
