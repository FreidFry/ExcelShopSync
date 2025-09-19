using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistance.Model;
using OfficeOpenXml;

namespace ExcelShSy.Infrastructure.Factories
{
    public class ExcelPageFactory : IExcelPageFactory
    {
        private readonly IColumnMappingStorage _columnMappingStorage;
        public ExcelPageFactory(IColumnMappingStorage columnMappingStorage)
        {
            _columnMappingStorage = columnMappingStorage;
        }

        public IExcelSheet Create(ExcelWorksheet worksheet)
        {
            var page = new ExcelPage(worksheet)
            {
                UnmappedHeaders = GetTempHeaders(worksheet)
            };
            page.MappedHeaders = GetRealHeaders(page.UnmappedHeaders);

            return page;
        }

        static Dictionary<string, int>? GetTempHeaders(ExcelWorksheet worksheet)
        {
            if (worksheet == null || worksheet.Dimension == null)
                return [];

            return worksheet.GetRowValueColumnMap(worksheet.Dimension.Start.Row);
        }

        Dictionary<string, int>? GetRealHeaders(Dictionary<string, int>? undefinedHeaders)
        {
            var template = _columnMappingStorage.Columns;

            return undefinedHeaders == null ? [] : template
            .SelectMany(pair => pair.Value, (pair, name) => new { pair.Key, Name = name })
            .Where(x => undefinedHeaders.ContainsKey(x.Name))
            .GroupBy(x => x.Key)
            .ToDictionary(
                g => g.Key,
                g => undefinedHeaders[g.First().Name]
            );
        }
    }
}
