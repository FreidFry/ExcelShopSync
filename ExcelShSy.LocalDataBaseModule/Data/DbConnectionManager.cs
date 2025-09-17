using Microsoft.Data.Sqlite;

namespace ExcelShSy.LocalDataBaseModule.Data
{
    public class DbConnectionManager
    {
        private string _connectionString;

        public DbConnectionManager(string path = "", bool createSubFolder = false)
        {
            SetDataBase(path, createSubFolder);
        }

        private static string CreateDbConnect(string path, bool createSubFolder)
        {
            if (string.IsNullOrEmpty(path))
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "F4Labs", "ExcelShSy", "LocalDataBase");
            else
                if (createSubFolder)
                path = Path.Combine(path, "ExcelShSy", "LocalDataBase");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        private void SetDataBase(string path, bool createSubFolder)
        {
            var dbFolderPath = CreateDbConnect(path, createSubFolder);
            var dbFilePath = Path.Combine(dbFolderPath, "Products.db");
            _connectionString = $"Data Source={dbFilePath}";

            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            InitProductTable();
            InitShopsTable();
            InitMappingTable();


            InitDeleteProductTrigger();
        }

        private void InitProductTable()
        {
            var sql =
            @"
                CREATE TABLE IF NOT EXISTS MasterProducts (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    MasterArticle TEXT UNIQUE NOT NULL
                );
            ";
            ExecuteQuery(sql);
        }

        private void InitShopsTable()
        {
            var sql =
            @"
                CREATE TABLE IF NOT EXISTS Shops (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                );
            ";
            ExecuteQuery(sql);
        }

        private void InitMappingTable()
        {
            var sql =
            @"
                CREATE TABLE IF NOT EXISTS ProductShopMapping (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    MasterProductId INTEGER NOT NULL,
                    ShopId INTEGER NOT NULL,
                    Article TEXT NOT NULL,
                    FOREIGN KEY (MasterProductId) REFERENCES MasterProducts(Id),
                    FOREIGN KEY (ShopId) REFERENCES Shops(Id) ON DELETE CASCADE
                );
            ";
            ExecuteQuery(sql);
        }

        private void InitDeleteProductTrigger()
        {
            var sql = @"
        CREATE TRIGGER IF NOT EXISTS DeleteProductIfNoMappings
        AFTER DELETE ON ProductShopMapping
        FOR EACH ROW
        BEGIN
            DELETE FROM MasterProducts
            WHERE Id = OLD.MasterProductId
            AND NOT EXISTS (
                SELECT 1 FROM ProductShopMapping WHERE MasterProductId = OLD.MasterProductId
            );
        END;
    ";
            ExecuteQuery(sql);
        }

        public void ExecuteQuery(string sql)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using (var pragma = connection.CreateCommand())
            {
                pragma.CommandText = "PRAGMA foreign_keys = ON;";
                pragma.ExecuteNonQuery();
            }

            using var command = connection.CreateCommand();
            command.CommandText = "PRAGMA foreign_keys = ON;";
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }
    }
}
