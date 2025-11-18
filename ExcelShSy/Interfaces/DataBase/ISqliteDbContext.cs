namespace ExcelShSy.Core.Interfaces.DataBase;

/// <summary>
/// Provides abstraction over a SQLite database context with helper methods for schema and data access.
/// </summary>
public interface ISqliteDbContext
{
    /// <summary>
    /// Executes a scalar SQL query and returns the first column of the first row in the result set.
    /// </summary>
    /// <param name="sql">The SQL statement to execute.</param>
    /// <returns>The scalar result, or <c>null</c> when no value is returned.</returns>
    string? ExecuteScalar(string sql);

    /// <summary>
    /// Releases the database context resources.
    /// </summary>
    void Dispose();

    /// <summary>
    /// Creates a new command with no predefined command text.
    /// </summary>
    /// <returns>The command wrapper for manual configuration.</returns>
    IDbCommandWrapper CreateCommand();

    /// <summary>
    /// Creates a new command initialized with the provided command text.
    /// </summary>
    /// <param name="commandText">The SQL command text.</param>
    /// <returns>The command wrapper ready for execution.</returns>
    IDbCommandWrapper CreateCommand(string commandText);

    /// <summary>
    /// Executes a SQL statement that does not return result sets (for example, INSERT or UPDATE).
    /// </summary>
    /// <param name="sql">The SQL statement to execute.</param>
    void ExecuteNonQuery(string sql);

    /// <summary>
    /// Renames a column in the specified table.
    /// </summary>
    /// <param name="table">The table that contains the column.</param>
    /// <param name="column">The current column name.</param>
    /// <param name="newColumn">The new column name.</param>
    void RenameColumn(string table, string column, string newColumn);

    /// <summary>
    /// Removes the specified column from the table.
    /// </summary>
    /// <param name="table">The table that contains the column.</param>
    /// <param name="column">The column name to remove.</param>
    void RemoveColumn(string table, string column);

    /// <summary>
    /// Creates a new column using the provided column name.
    /// </summary>
    /// <param name="columnName">The name of the column to create.</param>
    void CreateColumn(string columnName);
}