using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.LocalDataBaseModule.Data;
using Microsoft.Data.Sqlite;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using static ExcelShSy.LocalDataBaseModule.Extensions.DataExecuteRequest;
using static ExcelShSy.LocalDataBaseModule.Persistance.Enums;

namespace ExcelShSy.LocalDataBaseModule
{
    public partial class DataBaseViewer : Window
    {
        private readonly IShopStorage  _shopStorage;
        private readonly DataUpdateManager _updateManager;
        
        
        private readonly string _connectionString;
        public DataBaseViewer(IDataBaseInitializer connectionManager, IShopStorage shopStorage)
        {
            _shopStorage = shopStorage;

            _connectionString = connectionManager.GetConnectionString();
            _updateManager = new DataUpdateManager(_connectionString);
            InitializeComponent();
            this.Closing += Window_Closing;
            
            Initialize();
        }

        private void Initialize()
        {
            var sql = $@"SELECT * FROM {Tables.ProductShopMapping} ORDER BY Id DESC;";
            
            LoadToDataGrid(sql, Table, _connectionString, _shopStorage.GetShopList(), _updateManager);
        }

        private void AddNewProduct_Click(object? sender, RoutedEventArgs e)
        {
            var sql = $@"INSERT INTO {Tables.ProductShopMapping} (MasterArticle) VALUES ('Product')";
            try
            {
                ExecuteQuery(sql, _connectionString);
                Table.Columns.Clear();
                Initialize();
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                MessageBoxManager.GetMessageBoxStandard("error",
                    $"Its value issue",
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
            }
        }

        private void Window_Closing(object? sender, CancelEventArgs e)
        {
            _updateManager.FlushNow();
        }
    }
}
