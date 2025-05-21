using ExcelShSy.Core.Factorys;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Core.Services.Common;
using ExcelShSy.Core.Services.Operations;
using ExcelShSy.Core.Services.Storage;
using ExcelShSy.Infrastracture;
using ExcelShSy.Infrastracture.Persistance.Model;
using ExcelShSy.Infrastracture.Persistance.ShopData;

using Microsoft.Extensions.DependencyInjection;

namespace ExcelShSy.Core.AppConfigs
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<IFileStorage, FileStorage>();
            services.AddSingleton<IFileManager, FileManager>();
            services.AddScoped<IDataProduct, DataProduct>();

            services.AddScoped<IFileProvider, FileProvider>();

            services.AddScoped<IExcelFile, ExcelFile>();
            services.AddScoped<IExcelPage, ExcelPage>();
            services.AddScoped<IShopMappings, ShopMappings>();
            services.AddScoped<ILanguageDetector, LanguageDetector>();
            services.AddScoped<IGetPricesFromSource, GetPricesFromSource>();

            //factory
            services.AddScoped<IExcelFileFactory, ExcelFileFactory>();
            services.AddScoped<IExcelPageFactory, ExcelPageFactory>();
            
            services.AddSingleton<MainWindow>();
            return services;
        }
    }
}
