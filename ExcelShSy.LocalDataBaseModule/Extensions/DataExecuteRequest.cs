using System.Data;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using ExcelShSy.LocalDataBaseModule.Data;
using Microsoft.Data.Sqlite;
using static ExcelShSy.LocalDataBaseModule.Persistance.Enums;

namespace ExcelShSy.LocalDataBaseModule.Extensions;

public static class DataExecuteRequest
{
    public static void ExecuteQuery(string sql, string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using (var pragma = connection.CreateCommand())
        {
            pragma.CommandText = "PRAGMA foreign_keys = ON;";
            pragma.ExecuteNonQuery();
        }

        using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }

    public static void LoadToDataGrid(string sql, DataGrid grid, string connectionString, List<string> shopList, DataUpdateManager updateManager)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = sql;

        using var reader = command.ExecuteReader();
        var dataTable = new DataTable();

        dataTable.Load(reader);
        grid.AddColumn("MasterArticle", $"{MasterProductsColumns.MasterArticle}", updateManager);
        foreach (var shop in shopList)
        {
            grid.AddColumn(shop, $"{shop}", updateManager);
        }

        grid.ItemsSource = dataTable.DefaultView;
    }
    
    private static void AddColumn(this DataGrid grid, string columnName, string nameInDataBase, DataUpdateManager updateManager)
    {
        grid.Columns.Add(new DataGridTemplateColumn
        {
            Header = columnName, //column in db
            CellTemplate = CreateCellTemplate(nameInDataBase, updateManager), //data from db
            Tag = columnName //column in db
        });
    }

    private static FuncDataTemplate<DataRowView> CreateCellTemplate(string nameInDataBase, DataUpdateManager updateManager) => 
        new((row, ns) =>
        {
            var tb = new TextBox
            {
                Text = row.Row[nameInDataBase].ToString(), // value in db
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Tag = int.Parse(row.Row[$"{CommonColumns.Id}"].ToString()) //id in DB
            };

            tb.GetObservable(TextBox.TextProperty).Subscribe(newText =>
            {
                if (tb.Tag is int idSttr)
                    updateManager.ScheduleUpdate(idSttr, nameInDataBase, newText ?? "");
            });
            return tb;
        }, true);
}

