using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.LocalDataBaseModule;
using ExcelShSy.Ui.Interfaces;

namespace ExcelShSy.Ui.Factories;

public class DataBaseViewerFactory : IDataBaseViewerFactory
{
    private readonly IDataBaseInitializer _dataBaseInitializer;
    private readonly IShopStorage _shopStorage;
    private readonly ISqliteDbContext _sqliteDbContext;
    
    public DataBaseViewerFactory(IDataBaseInitializer dataBaseInitializer, IShopStorage shopStorage, ISqliteDbContext sqliteDbContext)
    {
        _dataBaseInitializer = dataBaseInitializer;
        _shopStorage = shopStorage;
        _sqliteDbContext = sqliteDbContext;
    }

    public DataBaseViewer Create() => new(_dataBaseInitializer, _shopStorage, _sqliteDbContext);
}