using ExcelShSy.Core.Interfaces.Excel;

namespace ExcelShSy.Core.Interfaces.Storage
{
    public interface IFileStorage
    {
        List<IExcelFile> Target { get; }
        List<IExcelFile> Source { get; }

        void AddTarget(List<IExcelFile> file);
        void AddSource(List<IExcelFile> file);

        void ClearTargetFiles();
        void ClearSourceFiles();

        void ClearAllFiles();
    }
}
