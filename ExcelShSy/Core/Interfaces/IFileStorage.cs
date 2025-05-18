namespace ExcelShSy.Core.Interfaces
{
    public interface IFileStorage
    {
        List<IExcelFile> Target { get; }
        List<IExcelFile> Source { get; }

        void AddTarget(List<IExcelFile> file);
        void AddSource(List<IExcelFile> file);

        void ClearTarget();
        void ClearSource();

        void ClearAll();

    }
}
