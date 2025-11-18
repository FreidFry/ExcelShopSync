namespace ExcelShSy.Core.Interfaces.DataBase;

/// <summary>
/// Provides helpers for retrieving product data from the application's database.
/// </summary>
public interface IDbGetDataManager
{
    /// <summary>
    /// Retrieves the master article identifier for the specified shop-specific article.
    /// </summary>
    /// <param name="article">The shop-specific article identifier.</param>
    /// <param name="shop">The shop name associated with the article.</param>
    /// <param name="connectionString">The database connection string.</param>
    /// <returns>The master article identifier, or <c>null</c> if not found.</returns>
    string? GetMasterArticle(string article, string shop, string connectionString);
}