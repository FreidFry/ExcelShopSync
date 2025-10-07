using ExcelShSy.Core.Interfaces.Excel;

namespace ExcelShSy.Core.Interfaces.Storage
{
    public interface IFileProvider
    {
        List<IExcelFile> FetchExcelFile(List<string> paths);
        IExcelFile FetchExcelFile(string paths);
        Task<List<string>?> PickExcelFilePaths();
        Task<string?> PickExcelFilePath();
    }
}
