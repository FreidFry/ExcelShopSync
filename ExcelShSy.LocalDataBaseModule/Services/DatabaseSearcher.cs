using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.LocalDataBaseModule.Persistance;

namespace ExcelShSy.LocalDataBaseModule.Services;

public class DatabaseSearcher(ISqliteDbContext context) : IDatabaseSearcher
{
    public string SearchProduct(string shopName, string productName)
    {
        var sql = $"SELECT \"{Enums.MappingColumns.MasterArticle}\" FROM \"{Enums.Tables.ProductShopMapping}\" WHERE {shopName} = '{productName}'";
        var result = context.ExecuteScalar(sql);
        return result ?? productName;
    }
}