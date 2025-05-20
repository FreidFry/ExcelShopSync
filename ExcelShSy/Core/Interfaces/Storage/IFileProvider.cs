using ExcelShSy.Core.Interfaces.Excel;

namespace ExcelShSy.Core.Interfaces.Storage
{
    public interface IFileProvider
    {
        List<IExcelFile> GetFiles(List<string> paths);
        List<string> GetPaths();
    }
}
