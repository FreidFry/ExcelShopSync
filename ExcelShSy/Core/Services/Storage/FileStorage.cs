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
            foreach (var file in files)
            {
                if (Target.Contains(file))
                    continue;
                Target.Add(file);
            }
        }
        public void AddSource(List<IExcelFile> files)
        {
            foreach (var file in files)
            {
                if (Source.Contains(file))
                    continue;
                Source.Add(file);
            }
        }

        public void ClearTarget() => Target.Clear();
        public void ClearSource() => Source.Clear();

        public void ClearAll()
        {
            Target.Clear();
            Target.TrimExcess();
            Source.Clear();
            Source.TrimExcess();
        }
    }
}
