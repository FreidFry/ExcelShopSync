using ExcelShSy.Core.Interfaces;
using ExcelShSy.Core.Interfaces.Common;
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
        private readonly ILocalizationService _localizationService;

        public EditLoadFilesWindowFactory(IFileManager fileManager, IFileProvider fileProvider, IExcelFileFactory excelFileFactory, ILocalizationService localizationService)
        {
            _FileManager = fileManager;
            _FileProvider = fileProvider;
            _ExcelFileFactory = excelFileFactory;
            _localizationService = localizationService;
        }

        public EditLoadFilesWindow Create(string page) =>
            new(page, _FileManager, _FileProvider, _ExcelFileFactory, _localizationService);
        public EditLoadFilesWindow Create() =>
            new(_FileManager, _FileProvider, _ExcelFileFactory, _localizationService);
    }
}
