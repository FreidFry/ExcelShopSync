using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Factories;
using ExcelShSy.Infrastructure.Persistence.Model;
using ExcelShSy.Infrastructure.Services;
using ExcelShSy.Infrastructure.Services.Common;
using ExcelShSy.Infrastructure.Services.Logger;
using ExcelShSy.Infrastructure.Services.Operations;
using ExcelShSy.Infrastructure.Services.Storage;
using ExcelShSy.LocalDataBaseModule;
using ExcelShSy.LocalDataBaseModule.Data;
using ExcelShSy.LocalDataBaseModule.Persistance;
using ExcelShSy.LocalDataBaseModule.Services;
using ExcelShSy.LocalDataBaseModule.Wrappers;
using ExcelShSy.Localization;
using ExcelShSy.Settings.Properties;
using ExcelShSy.Ui.AppConfigs;
using ExcelShSy.Ui.Factories;
using ExcelShSy.Ui.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Tests
{
    internal static class TestServicesDI
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            #region Singleton

            #region Sotorages

            services.AddSingleton<IFileStorage, FileStorage>();
            services.AddSingleton<IFileManager, FileManager>();
            services.AddSingleton<IProductStorage, ProductStorage>();
            services.AddSingleton<IShopStorage, ShopStorage>();
            services.AddSingleton<ISqliteDbContext, SqliteDbContext>();
            services.AddSingleton<IAppSettings, AppSettings>();

            #endregion

            services.AddSingleton<IColumnMappingStorage, ColumnMappingStorage>();
            services.AddSingleton<ILocalizationService, LocalizationService>();

            services.AddSingleton<IDataBaseInitializer, DbCreateManager>();

            services.AddSingleton<ILogger, Logger>();

            #endregion

            services.AddScoped<IFileProvider, FileProvider>();

            #region Models

            services.AddScoped<IExcelFile, ExcelFile>();
            services.AddScoped<IExcelSheet, ExcelPage>();
            services.AddScoped<IShopTemplate, ShopTemplate>();

            #endregion

            services.AddTransient<IConfigurationManager, ConfigManager>();

            services.AddScoped<ILanguageIdentifier, LanguageIdentifier>();
            services.AddScoped<IGetProductManager, ProductImporterSelector>();
            services.AddScoped<IFetchPriceListProduct, FetchProductPriceList>();
            services.AddScoped<IFetchMarketProduct, MarketProductImporter>();

            #region DataBase

            services.AddScoped<IDatabaseUpdateManager, SqliteUpdateManager>();
            services.AddScoped<IDataReaderWrapper, SqliteDataReaderWrapper>();
            services.AddScoped<IDatabaseSearcher, DatabaseSearcher>();

            #endregion

            #region Executes

            services.AddScoped<IExecuteOperation, SyncPrice>();
            services.AddScoped<SyncPrice>();
            services.AddScoped<IExecuteOperation, SyncQuantity>();
            services.AddScoped<SyncQuantity>();
            services.AddScoped<IExecuteOperation, SyncAvailability>();
            services.AddScoped<SyncAvailability>();
            services.AddScoped<IExecuteOperation, SyncDiscount>();
            services.AddScoped<SyncDiscount>();
            services.AddScoped<IExecuteOperation, SyncDiscountDate>();
            services.AddScoped<SyncDiscountDate>();
            services.AddScoped<IExecuteOperation, IncreasePricePercent>();
            services.AddScoped<IncreasePricePercent>();
            services.AddScoped<IExecuteOperation, SavePackages>();
            services.AddScoped<SavePackages>();
            services.AddScoped<IExecuteOperation, FindMissingProducts>();
            services.AddScoped<FindMissingProducts>();

            #endregion

            #region Factory
            services.AddTransient<IExcelFileFactory, ExcelFileFactory>();
            services.AddTransient<IExcelPageFactory, ExcelPageFactory>();
            services.AddTransient<IOperationTaskFactory, OperationTaskFactory>();
            services.AddTransient<IWindowFactory<DataBaseViewer>, DataBaseViewerFactory>();
            services.AddTransient<ICheckConnectionFactory, CheckConnectionFactory>();
            services.AddTransient<IUpdateManagerFactory, UpdateManagerFactory>();

            services.AddScoped<IShopTemplateFactory, ShopTemplateFactory>();

            services.AddTransient<IMessagesService<IMsBox<ButtonResult>>, MessageTest>();
            services.AddTransient<IMessagesCustomService<IMsBox<string>, MessageBoxCustomParams>, MessageTest>();

            #endregion
        }
    }
}
