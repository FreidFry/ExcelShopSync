using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.LocalDataBaseModule;
using ExcelShSy.LocalDataBaseModule.Data;
using ExcelShSy.Ui.Interfaces;

namespace ExcelShSy.Ui.Factories;

public class DataBaseViewerFactory : IDataBaseViewerFactory
{
    private readonly IDataBaseInitializer _dataBaseInitializer;
    private readonly IShopStorage _shopStorage;
    private readonly ISqliteDbContext _sqliteDbContext;
    private readonly IDatabaseUpdateManager _updateManager;
    
    public DataBaseViewerFactory(IDataBaseInitializer dataBaseInitializer, IShopStorage shopStorage, ISqliteDbContext sqliteDbContext, IDatabaseUpdateManager updateManager)
    {
        _dataBaseInitializer = dataBaseInitializer;
        _shopStorage = shopStorage;
        _sqliteDbContext = sqliteDbContext;
        _updateManager = updateManager;
    }

    public DataBaseViewer Create() => new(_dataBaseInitializer, _shopStorage, _sqliteDbContext, _updateManager);
}