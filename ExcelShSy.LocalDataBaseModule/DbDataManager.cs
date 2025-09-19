using ExcelShSy.Core.Interfaces.DataBase;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using static ExcelShSy.LocalDataBaseModule.Extensions.DataExecuteRequest;
using static ExcelShSy.LocalDataBaseModule.Persistance.Enums;

namespace ExcelShSy.LocalDataBaseModule
{
    public class DbDataManager(IDataBaseInitializer initializer)
    {
        private readonly string _connectionString = initializer.GetConnectionString();

        public void AddMasterProduct(string masterArticle)
        {
            AddNewValue($"{Tables.MasterProducts}", $"{MasterProductsColumns.MasterArticle}" ,masterArticle);
        }

        public void GetProducts()
        {
            var sql = $@"SELECT * FROM {Tables.MasterProducts}";
            
            ExecuteQuery(sql, _connectionString);
        }
        
        private void AddNewValue(string tableName, string columns, string values)
        {
            if (columns.Length != values.Length)
                MessageBoxManager.GetMessageBoxStandard("Error", "Длина columns и значения не совпадает", ButtonEnum.Ok, Icon.Warning);
            var sql = @$"INSERT INTO {tableName} ({columns}) VALUES ({values});";
            
            ExecuteQuery(sql, _connectionString);
        }
    }
}
