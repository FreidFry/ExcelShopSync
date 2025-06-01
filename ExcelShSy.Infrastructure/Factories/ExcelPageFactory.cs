using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistance.Model;
using ExcelShSy.Infrastructure.Persistance.ShopData.Mappings;

using OfficeOpenXml;

namespace ExcelShSy.Infrastructure.Factories
{
    public class ExcelPageFactory : IExcelPageFactory
    {
        public IExcelPage Create(ExcelWorksheet worksheet)
        {
            var page = new ExcelPage(worksheet)
            {
                UndefinedHeaders = GetTempHeaders(worksheet)
            };
            page.Headers = GetRealHeaders(page.UndefinedHeaders);

            return page;
        }

        static Dictionary<string, int>? GetTempHeaders(ExcelWorksheet worksheet)
        {
            if (worksheet == null || worksheet.Dimension == null)
                return [];

            return worksheet.GetRowValueColumnMap(worksheet.Dimension.Start.Row);
        }

        static Dictionary<string, int>? GetRealHeaders(Dictionary<string, int>? undefinedHeaders)
        {
            var template = ColumnMapping.Columns;

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
