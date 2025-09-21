using System.Data;

namespace ExcelShSy.Core.Interfaces.DataBase;

public interface IDataReaderWrapper : IDisposable
{
    bool Read();
    string GetString(int ordinal);
    IDataReader GetReader();
}