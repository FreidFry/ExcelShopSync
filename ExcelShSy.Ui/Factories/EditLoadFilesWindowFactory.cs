using ExcelShSy.Core.Helpers;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Ui.Interfaces;
using ExcelShSy.Ui.ModelView.View;
using ExcelShSy.Ui.Windows;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Ui.Factories
{
    public class EditLoadFilesWindowFactory(
        IFileManager fileManager,
        IFileProvider fileProvider,
        IExcelFileFactory excelFileFactory,
        ILocalizationService localizationService,
        IMessages<IMsBox<ButtonResult>> messages)
        : IEditLoadFilesWindowFactory
    {
        public EditLoadFilesWindow Create()
        {
            var model = new EditLoadFilesModel(
                fileManager,
                fileProvider,
                excelFileFactory,
                localizationService,
                messages);

            var window = new EditLoadFilesWindow(model);

            return window;
        }
    }
}
