namespace ExcelShSy.Core.Interfaces.DataBase;

/// <summary>
/// Provides methods for ensuring the database schema is prepared for application use.
/// </summary>
public interface IDataBaseInitializer
{
    /// <summary>
    /// Performs initialization routines such as schema creation or migration.
    /// </summary>
    void InitializeDatabase();
}