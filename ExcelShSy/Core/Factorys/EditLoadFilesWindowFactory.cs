using ExcelShSy.Core.Interfaces.Storage;

namespace ExcelShSy.Core.Factorys
{
    public class EditLoadFilesWindowFactory : IEditLoadFilesWindowFactory
    {
        private readonly IFileManager _FileManager;
        private readonly IFileProvider _FileProvider;

        public EditLoadFilesWindowFactory(IFileManager fileManager, IFileProvider fileProvider)
        {
            _FileManager = fileManager;
            _FileProvider = fileProvider;
        }

        public EditLoadFilesWindow Create(string name)
        {
            return new(_FileManager, _FileProvider, name);
        }
    }
}
