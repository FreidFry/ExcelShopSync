using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.LocalDataBaseModule;
using ExcelShSy.Ui.Interfaces;

namespace ExcelShSy.Ui.Factories;

public class DataBaseViewerFactory : IDataBaseViewerFactory
{
    private readonly IDataBaseInitializer _dataBaseInitializer;
    private readonly IShopStorage _shopStorage;
    
    public DataBaseViewerFactory(IDataBaseInitializer dataBaseInitializer, IShopStorage shopStorage)
    {
        _dataBaseInitializer = dataBaseInitializer;
        _shopStorage = shopStorage;
    }

    public DataBaseViewer Create() => new(_dataBaseInitializer, _shopStorage);
}