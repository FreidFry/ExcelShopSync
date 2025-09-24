using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.LocalDataBaseModule.Data;
using ExcelShSy.LocalDataBaseModule.Extensions;
using ExcelShSy.LocalDataBaseModule.Persistance.Models;
using Microsoft.Data.Sqlite;
using static ExcelShSy.LocalDataBaseModule.Persistance.Enums;

namespace ExcelShSy.LocalDataBaseModule
{
    public partial class DataBaseViewer : Window
    {
        private readonly DataUpdateManager _updateManager;
        private readonly ISqliteDbContext _connection;
        private readonly DataGridBuilder _gridBuilder;
        
        private ObservableCollection<DynamicRow> Rows { get; } = [];
        private int _productCounter;
        
        public DataBaseViewer(IDataBaseInitializer dataBaseInitializer, IShopStorage shopStorage, ISqliteDbContext sqliteDbContext)
        {
            dataBaseInitializer.InitializeDatabase();
            _connection = sqliteDbContext;
            _updateManager = new DataUpdateManager(_connection);
            _gridBuilder = new DataGridBuilder(_connection);
            InitializeComponent();
            
            Table.ItemsSource = Rows;
            _gridBuilder.CreateDataGridColumns(Table, shopStorage.GetShopList(), _updateManager);
            
            Initialize();
            Closing += Window_Closing;
        }

        private void Initialize()
        {
            var sql = $@"SELECT * FROM {Tables.ProductShopMapping} ORDER BY {MappingColumns.Id} DESC;";
            new DataGridLoader(_connection).LoadToDataGrid(sql, Rows);
            _productCounter = Rows.Count + 1;
        }

        private void AddNewProduct_Click(object? sender, RoutedEventArgs e)
        {
            _updateManager.FlushNow();
            _gridBuilder.AddNewProductRow(Table, Rows, ref _productCounter);
        }

        private void Window_Closing(object? sender, CancelEventArgs e)
        {
            _updateManager.FlushNow();
        }

        private void TextBox_OnTextChanging(object? sender, TextChangingEventArgs e)
        {
            if (sender is TextBox textBox)
                DataGridExtensions.SearchExtension(textBox, Table, Rows);
        }
    }
}
