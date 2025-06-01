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
using ExcelShSy.Ui.Utils;

using Microsoft.Extensions.DependencyInjection;

namespace ExcelShSy.Ui.AppConfigs
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<IFileStorage, FileStorage>();
            services.AddSingleton<IFileManager, FileManager>();
            services.AddScoped<IDataProduct, DataProduct>();

            services.AddScoped<IFileProvider, FileProvider>();

            services.AddSingleton<ILogger, Logger>();

            services.AddScoped<IExcelFile, ExcelFile>();
            services.AddScoped<IExcelPage, ExcelPage>();
            services.AddScoped<IShopMappings, ShopMappings>();
            services.AddScoped<ILanguageDetector, LanguageDetector>();
            services.AddScoped<IGetProductManager, GetProductFromSource>();
            services.AddScoped<IFromPrice, FromPrice>();
            services.AddScoped<IFromSource, FromSource>();

            //factory
            services.AddScoped<IExcelFileFactory, ExcelFileFactory>();
            services.AddScoped<IExcelPageFactory, ExcelPageFactory>();
            services.AddScoped<ITaskFactory, MyTaskFactory>();
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

            //UI
            services.AddSingleton<ILocalizationService, LocalizationService>();
            services.AddSingleton<LocalizationBinding>();
            services.AddTransient<EditLoadFilesWindow>();
            services.AddTransient<SettingWindow>();
            services.AddTransient<MainWindow>();
            return services;
        }
    }
}
