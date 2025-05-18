using ExcelShSy.Core.Interfaces;
using ExcelShSy.Core.Services;
using ExcelShSy.Infrastracture;
using ExcelShSy.Infrastracture.Persistance.Model;
using Microsoft.Extensions.DependencyInjection;

namespace ExcelShSy.Core.AppConfigs
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();

            services.AddScoped<IFileProvider, FileProvider>();
            services.AddTransient<IExcelFile, ExcelFile>();
            services.AddTransient<IExcelPage, ExcelPage>();

            services.AddSingleton<IFileStorage, FileStorage>();
            services.AddSingleton<IFileManager, FileManager>();


            return services;
        }
    }
}
