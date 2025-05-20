using ExcelShSy.Core.Factorys;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Core.Services;
using ExcelShSy.Infrastracture;
using ExcelShSy.Infrastracture.Persistance.Helpers;
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
            services.AddScoped<IAssistanceMethods, AssistanceMethods>();

            //factory
            services.AddScoped<IExcelFileFactory, ExcelFileFactory>();
            services.AddScoped<IExcelPageFactory, ExcelPageFactory>();
            
            services.AddSingleton<MainWindow>();
            return services;
        }
    }
}
