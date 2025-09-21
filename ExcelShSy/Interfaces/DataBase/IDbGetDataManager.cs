namespace ExcelShSy.Core.Interfaces.DataBase;

public interface IDbGetDataManager
{
    string? GetMasterArticle(string article, string shop, string connectionString);
}