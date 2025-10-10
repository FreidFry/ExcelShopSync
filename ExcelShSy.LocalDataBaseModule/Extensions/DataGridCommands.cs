using System.Collections.ObjectModel;
using Avalonia.Controls;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.LocalDataBaseModule.Persistance;
using ExcelShSy.LocalDataBaseModule.Persistance.Models;
using MsBox.Avalonia.ViewModels.Commands;

namespace ExcelShSy.LocalDataBaseModule.Extensions;

internal static class DataGridCommands
{
    internal static void CellEditEndedEvent(DataGridCellEditEndedEventArgs e, IDatabaseUpdateManager updateManager)
    {
        if (e.Row.DataContext is not DynamicRow dynamicRow) return;

        var columnName = e.Column.Tag.ToString()!;
        updateManager.ScheduleUpdate(dynamicRow.Id, columnName, dynamicRow[columnName] ?? "");
    }

    internal static RelayCommand ClearCellCommand(DataGrid grid, IDatabaseUpdateManager updateManager) => 
        new(_ =>
        {
            if (grid.SelectedItem is not DynamicRow row || grid.CurrentColumn?.Tag is not string columnName) return;
            if (grid.ItemsSource is not ObservableCollection<DynamicRow> rows) return;
                
            row[columnName] = null;
            var index = rows.IndexOf(row);
            if (index < 0) return;
            
            rows.RemoveAt(index);
            rows.Insert(index, row);
                        
            updateManager.ScheduleUpdate(row.Id, columnName, row[columnName] ?? "");
            updateManager.FlushNow();
                        
            grid.SelectedItem = row;
            grid.CurrentColumn = grid.Columns.FirstOrDefault(c => c.Tag?.ToString() == columnName);
            grid.Focus();
        },
        _ => !IsMasterColumn(grid));

    internal static RelayCommand RemoveProductCommand(DataGrid grid, ISqliteDbContext connection) => new(_ =>
    {
            if (grid.SelectedItem is not DynamicRow row || grid.CurrentColumn?.Tag is not string) return;
            if (grid.ItemsSource is not ObservableCollection<DynamicRow> rows) return;

            try
            {
                var sql = $@"DELETE FROM {Enums.Tables.ProductShopMapping} WHERE Id = @id;";
                var cmd = connection.CreateCommand(sql);
                cmd.AddParametersWithValue("@id", row.Id);
                cmd.ExecuteNonQuery();
                rows.Remove(row);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
    }, _ => IsMasterColumn(grid));
    
    private static bool IsMasterColumn(DataGrid grid) =>
        grid.CurrentColumn?.Tag?.ToString() == $"{Enums.MappingColumns.MasterArticle}";
        

    internal static RelayCommand CopyCommand(DataGrid grid) => 
        new(_ =>
        {
            if (grid.SelectedItem is null) return;
            var cell = grid.CurrentColumn.GetCellContent(grid.SelectedItem) as TextBlock;
            var rowText = cell?.Text ?? string.Empty;
            
            var top = TopLevel.GetTopLevel(grid);
            top?.Clipboard?.SetTextAsync(rowText);
        });
}