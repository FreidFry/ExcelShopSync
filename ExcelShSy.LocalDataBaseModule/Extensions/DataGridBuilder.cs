using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.LocalDataBaseModule.Data;
using ExcelShSy.LocalDataBaseModule.Persistance;
using ExcelShSy.LocalDataBaseModule.Persistance.Models;
using Microsoft.Data.Sqlite;
using static ExcelShSy.LocalDataBaseModule.Extensions.DataGridCommands;

namespace ExcelShSy.LocalDataBaseModule.Extensions;

internal class DataGridBuilder(ISqliteDbContext context)
{
    internal void CreateDataGridColumns(DataGrid grid, List<string> shopList, DataUpdateManager updateManager)
    {
        AddTextColumn(grid,"Master", $"{Enums.MappingColumns.MasterArticle}");
        foreach (var colName in shopList)
        {
            AddTextColumn(grid, colName, colName);
        }
        grid.ClipboardCopyMode = DataGridClipboardCopyMode.None;
        
        grid.KeyBindings.AddRange(
            new List<KeyBinding>
            {
                new()
                {
                    Gesture = new KeyGesture(Key.C, KeyModifiers.Control),
                    Command = CopyCommand(grid)
                    
                },
                new()
                {
                    Gesture = new KeyGesture(Key.Delete),
                    Command = ClearCellCommand(grid, updateManager)
                },
                new()
                {
                    Gesture = new KeyGesture(Key.Delete, KeyModifiers.Shift),
                    Command = RemoveProductCommand(grid, context)
                }
            });
        
        grid.ContextMenu = CreateContextMenu(grid, updateManager, context);
        grid.CellEditEnded += (_, e) => CellEditEndedEvent(e, updateManager);
    }
    
    private static void AddTextColumn(DataGrid grid, string columnName, string columnNameInBd)
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

    private static ContextMenu CreateContextMenu(DataGrid grid, DataUpdateManager updateManager, ISqliteDbContext context) =>
        new()
        {
            ItemsSource = new[]
            {
                new MenuItem
                {
                    Header = "Copy",
                    Command = CopyCommand(grid),
                    InputGesture = new KeyGesture(Key.C, KeyModifiers.Control),
                },
                new MenuItem
                {
                    Header = "Clear Cell",
                    Command = ClearCellCommand(grid, updateManager),
                    InputGesture = new KeyGesture(Key.Delete)
                },
                new MenuItem
                {
                    Header = "Remove Product",
                    Command = RemoveProductCommand(grid, context),
                    InputGesture = new KeyGesture(Key.Delete, KeyModifiers.Shift)
                }
            }
        };

    public void AddNewProductRow(DataGrid grid, ObservableCollection<DynamicRow> rows, ref int productCount)
    {
        try
            {
                var sql = $@"INSERT INTO {Enums.Tables.ProductShopMapping} (MasterArticle) VALUES ('Product{productCount++}');";
                context.ExecuteNonQuery(sql);
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) //Exist in the table
            {
                var existSql = $"""
                                SELECT {Enums.MappingColumns.MasterArticle} FROM {Enums.Tables.ProductShopMapping}
                                WHERE {Enums.MappingColumns.MasterArticle}
                                LIKE 'Product%'
                                ORDER BY {Enums.MappingColumns.Id} DESC
                                LIMIT 1;
                                """;
                var result = context.ExecuteScalar(existSql)!;
                productCount = int.Parse(result["Product".Length..]) + 1;
                var sql = $@"INSERT INTO {Enums.Tables.ProductShopMapping} (MasterArticle) VALUES ('Product{productCount++}');";
                context.ExecuteNonQuery(sql);
            }
            finally
            {
                var getLast = $@"SELECT * FROM {Enums.Tables.ProductShopMapping} ORDER BY {Enums.MappingColumns.Id} DESC;";
                var reader = context.CreateCommand(getLast).ExecuteReader().GetReader();
                reader.Read();
                
                var columnCount = reader.FieldCount;
                var columnNames = new string[columnCount];
                for (var i = 0; i < columnCount; i++)
                    columnNames[i] = reader.GetName(i);
                
                const string idParameter = nameof(Enums.MappingColumns.Id);
                var row = new DynamicRow
                {
                    Id = reader.GetInt32(reader.GetOrdinal(idParameter))
                };

                for (var i = 0; i < columnCount; i++)
                {
                    if (columnNames[i] == idParameter) continue;
                    row[columnNames[i]] = reader.IsDBNull(i) ? null : reader.GetValue(i).ToString();
                }
                rows.Insert(0, row);
                grid.SelectedItem = rows[0];
                grid.ScrollIntoView(rows.FirstOrDefault(), grid.Columns.FirstOrDefault());
            }
    }
}