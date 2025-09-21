using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Data;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.LocalDataBaseModule.Data;
using ExcelShSy.LocalDataBaseModule.Persistance.Models;
using static ExcelShSy.LocalDataBaseModule.Persistance.Enums;

namespace ExcelShSy.LocalDataBaseModule.Extensions;

public static class DataGridExtensions
{
    public static void LoadToDataGrid(string sql, ObservableCollection<DynamicRow> rows, ISqliteDbContext sqliteDbContext)
    {
        using var command = sqliteDbContext.CreateCommand(sql);
        using var reader = command.ExecuteReader().GetReader();
        
        var columnCount = reader.FieldCount;
        var columnNames = new string[columnCount];
        for (var i = 0; i < columnCount; i++)
            columnNames[i] = reader.GetName(i);

        while (reader.Read())
        {
            var row = new DynamicRow
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id"))
            };

            for (var i = 0; i < columnCount; i++)
            {
                if (columnNames[i] == "Id") continue;
                row[columnNames[i]] = reader.IsDBNull(i) ? null : reader.GetValue(i).ToString();
            }

            rows.Add(row);
        }
    }

    public static void CreateDataGridColumns(DataGrid grid, List<string> shopList, DataUpdateManager updateManager)
    {
        grid.AddTextColumn("Master" ,$"{MasterProductsColumns.MasterArticle}");
        foreach (var colName in shopList)
        {
            grid.AddTextColumn(colName, colName);
        }
        
        grid.CellEditEnded += (_, e) => CellEditEndedEvent(e, updateManager);
    }

    private static void CellEditEndedEvent(DataGridCellEditEndedEventArgs e, DataUpdateManager updateManager)
    {
        if (e.Row.DataContext is not DynamicRow dynamicRow) return;
        
        var columnName = e.Column.Tag.ToString()!;
            updateManager.ScheduleUpdate(dynamicRow.Id, columnName, dynamicRow[columnName] ?? "");
    }

    private static void AddTextColumn(this DataGrid grid, string columnName, string columnNameInBd)
    {
        grid.Columns.Add(new DataGridTextColumn
        {
            Tag = columnNameInBd,
            Header = columnName,
            Binding = new Binding($"[{columnNameInBd}]")
            {
                Mode = BindingMode.TwoWay
            }
        });
    }
}