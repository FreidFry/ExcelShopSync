using ExcelShSy.Core.Interfaces;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Factories;
using ExcelShSy.Infrastructure.Persistance.Model;
using ExcelShSy.Infrastructure.Persistance.ShopData;
using ExcelShSy.Infrastructure.Services;
using ExcelShSy.Infrastructure.Services.Common;
using ExcelShSy.Infrastructure.Services.Logger;
using ExcelShSy.Infrastructure.Services.Operations;
using ExcelShSy.Infrastructure.Services.Storage;
using ExcelShSy.Ui.Factories;
using ExcelShSy.Ui.Interfaces;
using ExcelShSy.Localization;

using Microsoft.Extensions.DependencyInjection;

namespace ExcelShSy.Ui.AppConfigs
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<IFileStorage, FileStorage>();
            services.AddSingleton<IFileManager, FileManager>();
            services.AddScoped<IProductStorage, ProductStorage>();

            services.AddScoped<IFileProvider, FileProvider>();

            services.AddSingleton<ILogger, Logger>();

            services.AddScoped<IExcelFile, ExcelFile>();
            services.AddScoped<IExcelSheet, ExcelPage>();
            services.AddScoped<IShopMapping, ShopMapping>();
            services.AddScoped<ILanguageIdentifier, LanguageIdentifier>();
            services.AddScoped<IGetProductManager, ProductImporterSelector>();
            services.AddScoped<IFetchMasterProduct, FetchProductMaster>();
            services.AddScoped<IFetchMarketProduct, MarketProductImporter>();

            //factory
            services.AddScoped<IExcelFileFactory, ExcelFileFactory>();
            services.AddScoped<IExcelPageFactory, ExcelPageFactory>();
            services.AddScoped<IOperationTaskFactory, OperationTaskFactory>();
            services.AddScoped<IEditLoadFilesWindowFactory, EditLoadFilesWindowFactory>();
            services.AddScoped<ISettingWindowFactory, SettingWindowFactory>();


            //Executes
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

            //UI
            services.AddTransient<ILocalizationManager, LocalizationManager>();
            services.AddTransient<EditLoadFilesWindow>();
            services.AddTransient<SettingWindow>();
            services.AddTransient<MainWindow>();
            return services;
        }
    }
}
