namespace ExcelShSy.Core.Interfaces.DataBase;

/// <summary>
/// Wraps database command functionality to provide a consistent interface across data providers.
/// </summary>
public interface IDbCommandWrapper : IDisposable
{
    /// <summary>
    /// Sets the SQL command text to execute.
    /// </summary>
    /// <param name="sql">The SQL statement.</param>
    void SetCommandText(string sql);

    /// <summary>
    /// Executes a non-query SQL command, returning the number of affected rows.
    /// </summary>
    /// <returns>The number of rows affected by the command.</returns>
    int ExecuteNonQuery();

    /// <summary>
    /// Executes the command and returns a reader for iterating results.
    /// </summary>
    /// <returns>A wrapper around the data reader.</returns>
    IDataReaderWrapper ExecuteReader();

    /// <summary>
    /// Executes the command and returns the first column of the first row in the result set.
    /// </summary>
    /// <returns>The scalar value, or <c>null</c> when no result is returned.</returns>
    object? ExecuteScalar();

    /// <summary>
    /// Adds a parameter with the provided value to the command.
    /// </summary>
    /// <param name="param">The parameter name.</param>
    /// <param name="value">The parameter value.</param>
    void AddParametersWithValue(string param, object value);

    /// <summary>
    /// Removes all parameters from the command.
    /// </summary>
    void ClearParameters();
}