using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;

namespace ExcelShSy.Infrastructure.Services.Storage
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

        public void ClearTargetFiles() => Target.Clear();
        public void ClearSourceFiles() => Source.Clear();

        public void ClearAllFiles()
        {
            Target.Clear();
            Source.Clear();
        }
    }
}
