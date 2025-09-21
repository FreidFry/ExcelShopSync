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
using static ExcelShSy.LocalDataBaseModule.Extensions.DataGridExtensions;

namespace ExcelShSy.LocalDataBaseModule
{
    public partial class DataBaseViewer : Window
    {
        private readonly DataUpdateManager _updateManager;
        private readonly ISqliteDbContext _connection;
        
        public ObservableCollection<DynamicRow> Rows { get; } = [];
        
        public DataBaseViewer(IDataBaseInitializer connectionManager, IShopStorage shopStorage, ISqliteDbContext sqliteDbContext)
        {
            _connection = sqliteDbContext;
            _updateManager = new DataUpdateManager(_connection);
            connectionManager.InitializeDatabase();
            InitializeComponent();
            
            Table.ItemsSource = Rows;
            CreateDataGridColumns(Table, shopStorage.GetShopList(), _updateManager);
            
            Initialize();
            this.Closing += Window_Closing;
        }

        private void Initialize()
        {
            var sql = $@"SELECT * FROM {Tables.ProductShopMapping} ORDER BY Id DESC;";
            LoadToDataGrid(sql, Rows, _connection);
        }

        private void AddNewProduct_Click(object? sender, RoutedEventArgs e)
        {
            _updateManager.FlushNow();
            var sql = $@"INSERT INTO {Tables.ProductShopMapping} (MasterArticle) VALUES ('Product')";
            try
            {
                _connection.ExecuteNonQuery(sql);
                Table.Columns.Clear();
                Initialize();
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) //Exist in the table
            {
                ErrorHelper.ShowError($"Its value issue");
            }
        }

        private void Window_Closing(object? sender, CancelEventArgs e)
        {
            _updateManager.FlushNow();
        }
    }
}
