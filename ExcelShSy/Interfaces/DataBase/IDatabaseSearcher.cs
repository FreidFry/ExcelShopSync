namespace ExcelShSy.Core.Interfaces.DataBase;

/// <summary>
/// Defines operations for locating product information in the database.
/// </summary>
public interface IDatabaseSearcher
{
    /// <summary>
    /// Searches for a product record by shop name and product identifier.
    /// </summary>
    /// <param name="shopName">The shop to search within.</param>
    /// <param name="productName">The product identifier or name.</param>
    /// <returns>The serialized product data or an empty string when not found.</returns>
    string SearchProduct(string shopName, string productName);
}