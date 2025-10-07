namespace ExcelShSy.Core.Interfaces.DataBase;

public interface IDatabaseUpdateManager
{
    void ScheduleUpdate(int rowId, string column, string newValue);
    void FlushNow();
}