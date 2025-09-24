using System.Data;
using ExcelShSy.Core.Interfaces.DataBase;
using Microsoft.Data.Sqlite;

namespace ExcelShSy.LocalDataBaseModule.Wrappers;

public class SqliteDataReaderWrapper : IDataReaderWrapper
{
    private readonly SqliteDataReader _reader;
    public SqliteDataReaderWrapper(SqliteDataReader reader) => _reader = reader;

    public bool Read() => _reader.Read();
    public IDataReader GetReader() => _reader;
    public string GetString(int ordinal) => _reader.GetString(ordinal);
    public void Dispose() => _reader.Dispose();
}