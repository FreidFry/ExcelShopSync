using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;

using System.IO;


namespace ExcelShSy.Infrastructure.Services
{
    [Task("SavePackages")]
    public class SavePackages : IExecuteOperation
    {

        private readonly IFileStorage _fileStorage;

        public SavePackages(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public void Execute()
        {
            foreach (var file in _fileStorage.Target)
                file.ExcelPackage.SaveAs(GetNewPath(file.FilePath));
        }

        static string GetNewPath(string oldPath)
        {
            var Extension = Path.GetExtension(oldPath);
            var path = Path.GetDirectoryName(oldPath);
            var name = Path.GetFileNameWithoutExtension(oldPath);
            return Path.Combine(path, $"{name}New{Extension}");
        }
    }
}
