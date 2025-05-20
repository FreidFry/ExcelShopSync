using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Infrastracture.Persistance.Model;
using ExcelShSy.Infrastracture.Persistance.ShopData;
using OfficeOpenXml;

namespace ExcelShSy.Core.Factorys
{
    public class ExcelPageFactory : IExcelPageFactory
    {
        readonly IAssistanceMethods _assistanceMethods;

        public ExcelPageFactory(IAssistanceMethods assistanceMethods)
        {
            _assistanceMethods = assistanceMethods;
        }

        public ExcelPage Create(ExcelWorksheet worksheet)
        {
            var page = new ExcelPage(worksheet)
            {
                UndefinedHeaders = GetTempHeaders(worksheet)
            };
            page.Headers = GetRealHeaders(page.UndefinedHeaders);

            return page;
        }

        Dictionary<string, int>? GetTempHeaders(ExcelWorksheet worksheet)
        {
            if (worksheet == null || worksheet.Dimension == null)
                return [];

            var dimension = worksheet.Dimension;

            var firstRow = dimension.Start.Row;
            var lastColumn = dimension.End.Column;

            return _assistanceMethods.GetRowValues(worksheet, firstRow, lastColumn);
        }

        static Dictionary<string, int>? GetRealHeaders(Dictionary<string, int>? undefinedHeaders)
        {
            var template = ColumnMapping.Columns;

            return undefinedHeaders == null ? [] : template
            .SelectMany(pair => pair.Value, (pair, name) => new { Key = pair.Key, Name = name })
            .Where(x => undefinedHeaders.ContainsKey(x.Name))
            .GroupBy(x => x.Key)
            .ToDictionary(
                g => g.Key,
                g => undefinedHeaders[g.First().Name]
            );
        }
    }
}
