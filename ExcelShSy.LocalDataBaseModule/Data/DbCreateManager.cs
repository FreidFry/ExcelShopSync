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
                using var cmd = sqliteDbContext.CreateCommand();
                var sqlCheck = $"PRAGMA table_info({Tables.ProductShopMapping})";
                cmd.SetCommandText(sqlCheck);
                var reader = cmd.ExecuteReader();

                var exists = false;
                while (reader.Read())
                {
                    if (reader.GetString(1) != shop) continue; // имя колонки в 2-й колонке
                    exists = true;
                    break;
                }
                reader.Dispose();

                if (exists) continue;
                var sql = $@"ALTER TABLE {Tables.ProductShopMapping} ADD COLUMN {shop} VARCHAR(100)";
                cmd.SetCommandText(sql);
                cmd.ExecuteNonQuery();
            }
        }

        private void InitMappingTable()
        {
            using var cmd = sqliteDbContext.CreateCommand();
            var sql = $"""
                       CREATE TABLE IF NOT EXISTS {Tables.ProductShopMapping} (
                           {MappingColumns.Id} INTEGER PRIMARY KEY AUTOINCREMENT,
                           MasterArticle VARCHAR(100) UNIQUE NOT NULL
                       );
                       """;
            cmd.SetCommandText(sql);
            cmd.ExecuteNonQuery();
        }

    }
}
