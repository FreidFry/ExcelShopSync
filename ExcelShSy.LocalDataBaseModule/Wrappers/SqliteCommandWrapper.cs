using ExcelShSy.Core.Interfaces.DataBase;
using Microsoft.Data.Sqlite;

namespace ExcelShSy.LocalDataBaseModule.Wrappers;

public class SqliteCommandWrapper : IDbCommandWrapper
{
    private readonly SqliteCommand _cmd;

    public SqliteCommandWrapper(SqliteConnection conn)
    {
        _cmd = conn.CreateCommand();
    }
    
    public void ClearParameters() => _cmd.Parameters.Clear();

    public void SetCommandText(string sql) => _cmd.CommandText = sql;
    public void AddParametersWithValue(string param, object value) => _cmd.Parameters.AddWithValue(param, value);
    public int ExecuteNonQuery()
    {
        var result = _cmd.ExecuteNonQuery();
        ClearParameters();
        return result;
    }

    public object? ExecuteScalar() => _cmd.ExecuteScalar();

    public IDataReaderWrapper ExecuteReader() => new SqliteDataReaderWrapper(_cmd.ExecuteReader());

    public void Dispose() => _cmd.Dispose();

}