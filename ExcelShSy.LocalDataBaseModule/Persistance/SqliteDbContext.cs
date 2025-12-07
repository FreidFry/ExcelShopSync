using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.LocalDataBaseModule.Wrappers;
using Microsoft.Data.Sqlite;
using static ExcelShSy.LocalDataBaseModule.Persistance.Enums;

namespace ExcelShSy.LocalDataBaseModule.Persistance;

public class SqliteDbContext : ISqliteDbContext, IDisposable
{
    private SqliteConnection _connection = null!;
    private readonly IAppSettings _appSettings;
    private readonly List<string> availableColumn = [];

    public SqliteDbContext(IAppSettings appSettings)
    {
        _appSettings = appSettings;
        OpenConnection();
        _appSettings.SettingsChanged += MoveDb;
    }

    public void CreateColumn(string columnName)
    {
        if (availableColumn.Contains(columnName)) return;
        using var cmd = _connection.CreateCommand();
        var sqlCheck = $"PRAGMA table_info({Tables.ProductShopMapping})";
        cmd.CommandText = sqlCheck;
        var reader = cmd.ExecuteReader();

        var exists = false;
        while (reader.Read())
        {
            if (reader.GetString(1) != columnName) continue; // имя колонки в 2-й колонке
            exists = true;
            availableColumn.Add(columnName);
            break;
        }
        reader.Dispose();

        if (exists) return;
        var sql = $"ALTER TABLE \"{Tables.ProductShopMapping}\" ADD COLUMN \"{columnName}\" VARCHAR(100)";
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    public void RenameColumn(string table, string column, string newColumn)
    {
        var sql = $"ALTER TABLE \"{table}\" RENAME COLUMN \"{column}\" TO \"{newColumn}\"";
        using var command = _connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }

    public void DropColumn(string table, string column)
    {
        var sql = $"UPDATE \"{table}\" SET \"{column}\" = NULL";
        using var command = _connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }
    
    private void OpenConnection()
    {
        var dbFile = Path.Combine(_appSettings.DataBasePath, "Products.db");
        if (!Directory.Exists(_appSettings.DataBasePath))
            Directory.CreateDirectory(_appSettings.DataBasePath);
        
        _connection = new SqliteConnection($"Data Source={dbFile}");
        _connection.Open();

        using var pragma = _connection.CreateCommand();
        pragma.CommandText = "PRAGMA foreign_keys = ON;";
        pragma.ExecuteNonQuery();
    }

    private void MoveDb()
    {
        var currentPath = _connection.DataSource;
        var finalPath = Path.Combine(_appSettings.DataBasePath, "Products.db");
        if (currentPath == finalPath) return;
        _connection.Dispose();
        File.Move(currentPath, finalPath, true);
        OpenConnection();
    }

    public IDbCommandWrapper CreateCommand() => new SqliteCommandWrapper(_connection);
    
    public IDbCommandWrapper CreateCommand(string commandText)
    {
        var command = new SqliteCommandWrapper(_connection);
        command.SetCommandText(commandText);
        return command;
    }

    public string? ExecuteScalar(string sql)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = sql;
        return command.ExecuteScalar()?.ToString();
    }
    
    public void ExecuteNonQuery(string sql)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }

    public void Dispose()
    {
        _connection.Dispose();
        _appSettings.SettingsChanged -= MoveDb;
    }
}