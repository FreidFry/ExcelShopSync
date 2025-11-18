namespace ExcelShSy.Core.Interfaces.DataBase;

/// <summary>
/// Handles batching and executing database updates for synchronized product data.
/// </summary>
public interface IDatabaseUpdateManager
{
    /// <summary>
    /// Schedules an update for the specified row and column.
    /// </summary>
    /// <param name="rowId">The row identifier to update.</param>
    /// <param name="column">The column to modify.</param>
    /// <param name="newValue">The new value to persist.</param>
    void ScheduleUpdate(int rowId, string column, string newValue);

    /// <summary>
    /// Immediately executes all staged updates.
    /// </summary>
    void FlushNow();
}