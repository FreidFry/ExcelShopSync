using ExcelShSy.LocalDataBaseModule.Persistance;
using Microsoft.Data.Sqlite;
using Timer = System.Timers.Timer;

namespace ExcelShSy.LocalDataBaseModule.Data;

public class DataUpdateManager(string connectionString)
{
    private readonly object _lock = new();
    private readonly Dictionary<(int RowId, string Column), string> _pendingChanges = new();
    private Timer? _timer;

    public void ScheduleUpdate(int rowId, string column, string newValue)
    {
        lock (_lock)
        {
            // сохраняем последнее значение
            _pendingChanges[(rowId, column)] = newValue;

            // сбрасываем/перезапускаем общий таймер
            _timer?.Stop();
            _timer = new Timer(5000); // 5 секунд
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

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        foreach (var ((rowId, column), value) in changesCopy)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = $"UPDATE {Enums.Tables.ProductShopMapping} SET {column} = @val WHERE Id = @id";
            cmd.Parameters.AddWithValue("@val", value);
            cmd.Parameters.AddWithValue("@id", rowId);
            cmd.ExecuteNonQuery();
        }
    }
}