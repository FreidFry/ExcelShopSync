using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Shop;
using static ExcelShSy.LocalDataBaseModule.Persistance.Enums;

namespace ExcelShSy.LocalDataBaseModule.Data
{
    public class DbCreateManager(IShopStorage shopStorage, ISqliteDbContext sqliteDbContext) : IDataBaseInitializer
    {
        public void InitializeDatabase()
        {
            InitMappingTable();
            AddShopsColumnToMapping();
        }

        private void AddShopsColumnToMapping()
        {
            foreach (var shop in shopStorage.GetShopList())
            {
                // Проверяем, есть ли колонка
                sqliteDbContext.CreateColumn(shop);
            }
        }

        private void InitMappingTable()
        {
            using var cmd = sqliteDbContext.CreateCommand();
            var sql = $"""
                       CREATE TABLE IF NOT EXISTS "{Tables.ProductShopMapping}" (
                           "{MappingColumns.Id}" INTEGER PRIMARY KEY AUTOINCREMENT,
                           MasterArticle VARCHAR(100) UNIQUE NOT NULL
                       );
                       """;
            cmd.SetCommandText(sql);
            cmd.ExecuteNonQuery();
        }

    }
}
