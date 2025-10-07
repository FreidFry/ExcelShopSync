using System.Collections.ObjectModel;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.LocalDataBaseModule.Persistance;
using ExcelShSy.LocalDataBaseModule.Persistance.Models;

namespace ExcelShSy.LocalDataBaseModule.Extensions;

public class DataGridLoader(ISqliteDbContext sqliteDbContext)
{
    public void LoadToDataGrid(string sql, ObservableCollection<DynamicRow> rows)
    {
        using var reader = sqliteDbContext.CreateCommand(sql).ExecuteReader().GetReader();

        var columnCount = reader.FieldCount;
        var columnNames = new string[columnCount];
        for (var i = 0; i < columnCount; i++)
            columnNames[i] = reader.GetName(i);

        while (reader.Read())
        {
            const string sortOrder = nameof(Enums.MappingColumns.Id);
            var row = new DynamicRow
            {
                Id = reader.GetInt32(reader.GetOrdinal(sortOrder))
            };

            for (var i = 0; i < columnCount; i++)
            {
                if (columnNames[i] == sortOrder) continue;
                row[columnNames[i]] = reader.IsDBNull(i) ? null : reader.GetValue(i).ToString();
            }

            rows.Add(row);
        }
    }

    
}