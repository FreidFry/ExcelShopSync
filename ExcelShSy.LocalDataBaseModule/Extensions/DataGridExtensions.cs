using System.Collections.ObjectModel;
using Avalonia.Controls;
using ExcelShSy.LocalDataBaseModule.Persistance.Models;

namespace ExcelShSy.LocalDataBaseModule.Extensions;

public static class DataGridExtensions
{
    public static void SearchExtension(TextBox textBox, DataGrid dataGrid, ObservableCollection<DynamicRow> rows)
    {
        var filter = textBox.Text;
        if (string.IsNullOrWhiteSpace(filter))
        {
            dataGrid.ItemsSource = rows;
            return;
        }
        var filtered = rows.Where(r =>
            r.Values.Any(v =>
                v != null && v.Contains(filter, StringComparison.OrdinalIgnoreCase)));
        dataGrid.ItemsSource = new ObservableCollection<DynamicRow>(filtered);
    }
}