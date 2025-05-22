using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;

namespace ExcelShSy.Core.Services.Storage
{
    public class FileStorage : IFileStorage
    {
        public List<IExcelFile> Target { get; } = [];
        public List<IExcelFile> Source { get; } = [];



        public void AddTarget(List<IExcelFile> files)
        {
            Target.AddRange(files);
        }
        public void AddSource(List<IExcelFile> files)
        {
            Source.AddRange(files);
        }

        public void ClearTarget() => Target.Clear();
        public void ClearSource() => Source.Clear();

        public void ClearAll()
        {
            Target.Clear();
            Source.Clear();
        }
    }
}
