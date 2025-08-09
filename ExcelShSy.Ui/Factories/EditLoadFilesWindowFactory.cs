using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Ui.Interfaces;

namespace ExcelShSy.Ui.Factories
{
    public class EditLoadFilesWindowFactory : IEditLoadFilesWindowFactory
    {
        private readonly IFileManager _FileManager;
        private readonly IFileProvider _FileProvider;
        private readonly IExcelFileFactory _ExcelFileFactory;

        public EditLoadFilesWindowFactory(IFileManager fileManager, IFileProvider fileProvider, IExcelFileFactory excelFileFactory)
        {
            _FileManager = fileManager;
            _FileProvider = fileProvider;
            _ExcelFileFactory = excelFileFactory;
        }

        public EditLoadFilesWindow Create(string page) => 
            new(page, _FileManager, _FileProvider, _ExcelFileFactory);
    }
}
