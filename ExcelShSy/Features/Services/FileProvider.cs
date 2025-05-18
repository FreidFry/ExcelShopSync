using ExcelShSy.Features.Interfaces;
using ExcelShSy.Infrastracture.Persistance.Interfaces;
using ExcelShSy.Infrastracture.Persistance.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace ExcelShSy.Features.Services
{
    public class FileProvider : IFileProvider
    {
        private readonly IServiceProvider _provider;

        public FileProvider(IServiceProvider provider)
        {
            _provider = provider;
        }

        public List<IExcelFile> GetFiles(List<string> paths)
        {
            var result = new List<IExcelFile>();

            foreach (var path in paths)
            {
                var instance = ActivatorUtilities.CreateInstance(_provider, typeof(ExcelFile), path) as IExcelFile;
                if (instance != null)
                    result.Add(instance);
            }

            return result;
        }

        public List<string> GetPaths()
        {
            OpenFileDialog fileDialog = new()
            {
                Filter = "Excel Files|*.xls;*.xlsx;*.xlsm",
                Multiselect = true
            };

            if (fileDialog.ShowDialog() == false || fileDialog.FileNames.Length == 0)
                return [];

            var result = new List<string>();

            foreach (var fileName in fileDialog.FileNames)
            {
                result.Add(fileName);
            }

            return result;
        }
    }
}
