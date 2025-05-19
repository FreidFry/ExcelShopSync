using ExcelShSy.Core.Interfaces;
using ExcelShSy.Core.Services;
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

            services.AddScoped<IFileProvider, FileProvider>();

            services.AddScoped<IExcelFile, ExcelFile>();
            services.AddScoped<IExcelPage, ExcelPage>();
            services.AddScoped<IShopMappings, ShopMappings>();
            
            services.AddSingleton<MainWindow>();
            return services;
        }
    }
}
