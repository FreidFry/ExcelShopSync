using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.LocalDataBaseModule.Extensions;
using ExcelShSy.LocalDataBaseModule.Persistance;
using Microsoft.Data.Sqlite;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using Timer = System.Timers.Timer;

namespace ExcelShSy.LocalDataBaseModule.Data;

public class SqliteUpdateManager(ISqliteDbContext dbContext, IMessages<IMsBox<ButtonResult>> messages) : IDatabaseUpdateManager
{
    private readonly IDbCommandWrapper _cmd = dbContext.CreateCommand();
    private readonly object _lock = new();
    private readonly Dictionary<(int RowId, string Column), string> _pendingChanges = new();
    private Timer? _timer;

    public void ScheduleUpdate(int rowId, string column, string newValue)
    {
        lock (_lock)
        {
            _pendingChanges[(rowId, column)] = newValue;

            _timer?.Stop();
            _timer = new Timer(5000);
            _timer.Elapsed += (_, _) => Flush();
            _timer.AutoReset = false;
            _timer.Start();
        }
    }

    public void FlushNow()
    {
        Flush();
    }

    private void Flush()
    {
        Dictionary<(int, string), string> changesCopy;
        lock (_lock)
        {
            if (_pendingChanges.Count == 0) return;
            changesCopy = new Dictionary<(int, string), string>(_pendingChanges);
            _pendingChanges.Clear();
        }
        
        foreach (var ((rowId, column), value) in changesCopy)
        {
            try
            {
                var updateQuery = $"UPDATE \"{Enums.Tables.ProductShopMapping}\" SET \"[{column}]\" = @val WHERE Id = @id;";
                _cmd.SetCommandText(updateQuery);

                _cmd.ClearParameters();
                _cmd.AddParametersWithValue("@val", value.Trim());
                _cmd.AddParametersWithValue("@id", rowId);

                _cmd.ExecuteNonQuery();
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) //Exist in the table
            {
                new ErrorHelper(messages).ShowError(ex.Message);
            }
        }
    }
}