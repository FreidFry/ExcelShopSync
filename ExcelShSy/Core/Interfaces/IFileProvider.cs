namespace ExcelShSy.Core.Interfaces
{
    public interface IFileProvider
    {
        List<IExcelFile> GetFiles(List<string> paths);
        List<string> GetPaths();
    }
}
