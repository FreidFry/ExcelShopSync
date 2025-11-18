using System.Data;

namespace ExcelShSy.Core.Interfaces.DataBase;

/// <summary>
/// Wraps data reader functionality to simplify disposal and access patterns.
/// </summary>
public interface IDataReaderWrapper : IDisposable
{
    /// <summary>
    /// Advances the reader to the next record.
    /// </summary>
    /// <returns><c>true</c> if another record is available; otherwise, <c>false</c>.</returns>
    bool Read();

    /// <summary>
    /// Retrieves the string value from the specified column ordinal.
    /// </summary>
    /// <param name="ordinal">The zero-based column ordinal.</param>
    /// <returns>The column value.</returns>
    string GetString(int ordinal);

    /// <summary>
    /// Gets the underlying data reader instance.
    /// </summary>
    /// <returns>The wrapped <see cref="IDataReader"/>.</returns>
    IDataReader GetReader();

    /// <summary>
    /// Releases the reader and all associated resources.
    /// </summary>
    new void Dispose();
}