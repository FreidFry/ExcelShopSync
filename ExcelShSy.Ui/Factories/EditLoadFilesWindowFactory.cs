using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Ui.Interfaces;
using ExcelShSy.Ui.Windows;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Ui.Factories
{
    public class EditLoadFilesWindowFactory(
        IFileManager fileManager,
        IFileProvider fileProvider,
        IExcelFileFactory excelFileFactory,
        ILocalizationService localizationService, IMessages<IMsBox<ButtonResult>> messages)
        : IEditLoadFilesWindowFactory
    {
        public EditLoadFilesWindow Create(string page) =>
            new(page, fileManager, fileProvider, excelFileFactory, localizationService, messages);
        public EditLoadFilesWindow Create() =>
            new(fileManager, fileProvider, excelFileFactory, localizationService, messages);
    }
}
