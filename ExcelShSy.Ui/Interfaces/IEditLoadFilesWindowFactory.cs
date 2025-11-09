using ExcelShSy.Ui.Windows;

namespace ExcelShSy.Ui.Interfaces
{
    public interface IEditLoadFilesWindowFactory
    {
        EditLoadFilesWindow Create(string name);
        EditLoadFilesWindow Create();
    }
}