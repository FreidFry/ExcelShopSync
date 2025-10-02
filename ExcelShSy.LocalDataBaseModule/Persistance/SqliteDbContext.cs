using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.LocalDataBaseModule.Wrappers;
using Microsoft.Data.Sqlite;

namespace ExcelShSy.LocalDataBaseModule.Persistance;

public class SqliteDbContext : ISqliteDbContext, IDisposable
{
    private SqliteConnection _connection = null!;
    private readonly IAppSettings _appSettings;

    public SqliteDbContext(IAppSettings appSettings)
    {
        _appSettings = appSettings;
        OpenConnection();
        _appSettings.SettingsChanged += MoveDb;
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
        _connection.Dispose();
        File.Move(currentPath, Path.Combine(_appSettings.DataBasePath, "Products.db"), true);
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