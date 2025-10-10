using System.Data;
using ExcelShSy.Core.Interfaces.DataBase;
using Microsoft.Data.Sqlite;

namespace ExcelShSy.LocalDataBaseModule.Wrappers;

public class SqliteDataReaderWrapper(SqliteDataReader reader) : IDataReaderWrapper
{
    public bool Read() => reader.Read();
    public IDataReader GetReader() => reader;
    public string GetString(int ordinal) => reader.GetString(ordinal);
    public void Dispose() => reader.Dispose();
}