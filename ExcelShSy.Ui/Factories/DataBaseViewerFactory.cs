using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.LocalDataBaseModule;
using ExcelShSy.Ui.Interfaces;

namespace ExcelShSy.Ui.Factories;

public class DataBaseViewerFactory(
    IDataBaseInitializer dataBaseInitializer,
    IShopStorage shopStorage,
    ISqliteDbContext sqliteDbContext,
    IDatabaseUpdateManager updateManager)
    : IWindowFactory<DataBaseViewer>
{
    public DataBaseViewer Create() => new(dataBaseInitializer, shopStorage, sqliteDbContext, updateManager);
}