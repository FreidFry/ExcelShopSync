using ExcelShSy.Infrastracture.Persistance.Interfaces;

namespace ExcelShSy.Features.Interfaces
{
    public interface IFileProvider
    {
        List<IExcelFile> GetFiles(List<string> paths);
        List<string> GetPaths();
    }
}
