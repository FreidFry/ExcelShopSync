namespace ExcelShSy.Core.Interfaces.DataBase;

public interface ISqliteDbContext
{
    string? ExecuteScalar(string sql);
    void Dispose();


    IDbCommandWrapper CreateCommand();
    IDbCommandWrapper CreateCommand(string commandText);
    void ExecuteNonQuery(string sql);
}