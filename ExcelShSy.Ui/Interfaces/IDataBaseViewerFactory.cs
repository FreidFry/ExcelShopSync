using ExcelShSy.LocalDataBaseModule;

namespace ExcelShSy.Ui.Interfaces;

public interface IDataBaseViewerFactory
{
    DataBaseViewer Create();
}