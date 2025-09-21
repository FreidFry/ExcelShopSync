namespace ExcelShSy.Core.Interfaces.DataBase;

public interface IDbCommandWrapper : IDisposable
{
    void SetCommandText(string sql);
    int ExecuteNonQuery();
    IDataReaderWrapper ExecuteReader();
    object? ExecuteScalar();
    void AddParametersWithValue(string param, object value);
    void ClearParameters();
}