using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Shop;
using Microsoft.Data.Sqlite;
using static ExcelShSy.LocalDataBaseModule.Extensions.DataExecuteRequest;
using static ExcelShSy.LocalDataBaseModule.Persistance.Enums;

namespace ExcelShSy.LocalDataBaseModule.Data
{
    public class DbCreateManager : IDataBaseInitializer
    {
        private string _connectionString;
        
        private readonly IAppSettings _appSettings;
        private readonly IShopStorage _shopStorage;

        public DbCreateManager(IAppSettings appSettings, IShopStorage shopStorage, bool createSubFolder = false)
        {
            _appSettings = appSettings;
            _shopStorage = shopStorage;
            _connectionString = CreateConnection(_appSettings.DataBasePath);
            InitializeDatabase();
        }
        
        public string GetConnectionString() => _connectionString;

        private static string CreateDbConnect(string path)
        {
            if (string.IsNullOrEmpty(path))
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "F4Labs");
            
            path = Path.Combine(path, "ExcelShSy", "LocalDataBase");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        private string CreateConnection(string path)
        {
            var dbFolderPath = CreateDbConnect(path);
            var dbFilePath = Path.Combine(dbFolderPath, "Products.db");
            return $"Data Source={dbFilePath}";
        }

        private void InitializeDatabase()
        {
          //  InitProductTable();
          //  InitShopsTable();
            InitMappingTable();

           // InitDeleteProductTrigger();

           // AddShopsToTable();
            AddShopsColumnToMapping();
        }

        private void InitProductTable()
        {
            var sql = $"""
                       CREATE TABLE IF NOT EXISTS {Tables.MasterProducts} (
                           {CommonColumns.Id} INTEGER PRIMARY KEY AUTOINCREMENT,
                           {MasterProductsColumns.MasterArticle} VARCHAR(100) UNIQUE NOT NULL
                       );
                       """;
            ExecuteQuery(sql, _connectionString);
        }

        private void AddShopsToTable()
        {
            var shopList = _shopStorage.GetShopList();
            var values = string.Join(", ", shopList.Select(s => $"('{s}')"));

            var sql = @$"INSERT INTO {Tables.Shops} ({ShopsColumns.Name}) VALUES {values};";
            ExecuteQuery(sql, _connectionString);
        }

        private void AddShopsColumnToMapping()
        {
            foreach (var shop in _shopStorage.GetShopList())
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

// Проверяем, есть ли колонка
                using var cmd = connection.CreateCommand();
                cmd.CommandText = $"PRAGMA table_info({Tables.ProductShopMapping})";
                using var reader = cmd.ExecuteReader();

                var exists = false;
                while (reader.Read())
                {
                    if (reader.GetString(1) != shop) continue; // имя колонки в 2-й колонке
                    exists = true;
                    break;
                }

                if (exists) continue;
                var sql = $@"ALTER TABLE {Tables.ProductShopMapping} ADD COLUMN {shop} VARCHAR(100)";
                ExecuteQuery(sql, _connectionString);
            }
        }

        private void InitShopsTable()
        {
            var sql = $"""
                       CREATE TABLE IF NOT EXISTS {Tables.Shops} (
                           {CommonColumns.Id} INTEGER PRIMARY KEY AUTOINCREMENT,
                           {ShopsColumns.Name} VARCHAR(100) NOT NULL
                       );
                       """;
            ExecuteQuery(sql, _connectionString);
        }

        /*
        private void InitMappingTable()
        {
            var sql = $"""
                       CREATE TABLE IF NOT EXISTS {Tables.ProductShopMapping} (
                           {CommonColumns.Id} INTEGER PRIMARY KEY AUTOINCREMENT,
                           {ProductsMappingColumns.MasterProductId} INTEGER NOT NULL,
                           {ProductsMappingColumns.ShopId} INTEGER NOT NULL,
                           {ProductsMappingColumns.Article} VARCHAR(100) NOT NULL,
                           FOREIGN KEY ({ProductsMappingColumns.MasterProductId}) REFERENCES {Tables.MasterProducts}({CommonColumns.Id}),
                           FOREIGN KEY ({ProductsMappingColumns.ShopId}) REFERENCES {Tables.Shops}({CommonColumns.Id}) ON DELETE CASCADE
                       );
                       """;
            ExecuteQuery(sql, _connectionString);
        }
        */
        
        private void InitMappingTable()
        {
            var sql = $"""
                       CREATE TABLE IF NOT EXISTS {Tables.ProductShopMapping} (
                           {CommonColumns.Id} INTEGER PRIMARY KEY AUTOINCREMENT,
                           MasterArticle VARCHAR(100) UNIQUE NOT NULL
                       );
                       """;
            ExecuteQuery(sql, _connectionString);
        }

        private void InitDeleteProductTrigger()
        {
            var sql = $"""
                       CREATE TRIGGER IF NOT EXISTS DeleteProductIfNoMappings
                       AFTER DELETE ON {Tables.ProductShopMapping}
                       FOR EACH ROW
                       BEGIN
                           DELETE FROM {Tables.MasterProducts}
                           WHERE {CommonColumns.Id} = OLD.{ProductsMappingColumns.MasterProductId}
                           AND NOT EXISTS (
                               SELECT 1 FROM {Tables.ProductShopMapping} WHERE {ProductsMappingColumns.MasterProductId} = OLD.{ProductsMappingColumns.MasterProductId}
                           );
                       END;
                       """;
            ExecuteQuery(sql, _connectionString);
        }

    }
}
