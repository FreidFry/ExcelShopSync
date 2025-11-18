using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistence.Model;
using OfficeOpenXml;

namespace ExcelShSy.Infrastructure.Factories
{
    /// <summary>
    /// Creates sheet abstractions and resolves column mappings using configured templates.
    /// </summary>
    public class ExcelPageFactory(IColumnMappingStorage columnMappingStorage) : IExcelPageFactory
    {
        /// <inheritdoc />
        public IExcelSheet Create(ExcelWorksheet worksheet)
        {
            var page = new ExcelPage(worksheet)
            {
                UnmappedHeaders = GetTempHeaders(worksheet)
            };
            page.MappedHeaders = GetRealHeaders(page.UnmappedHeaders);

            return page;
        }

        /// <summary>
        /// Retrieves the raw header map from the first row of the worksheet.
        /// </summary>
        /// <param name="worksheet">The worksheet to inspect.</param>
        /// <returns>A dictionary mapping header names to column indexes.</returns>
        private static Dictionary<string, int>? GetTempHeaders(ExcelWorksheet? worksheet)
        {
            if (worksheet?.Dimension == null)
                return [];

            return worksheet.GetRowValueColumnMap(worksheet.Dimension.Start.Row);
        }

        /// <summary>
        /// Generates mapped headers by matching discovered headers with known column templates.
        /// </summary>
        /// <param name="undefinedHeaders">The headers discovered from the worksheet.</param>
        /// <returns>A dictionary keyed by logical column name with associated column indexes.</returns>
        private Dictionary<string, int> GetRealHeaders(Dictionary<string, int>? undefinedHeaders)
        {
            var template = columnMappingStorage.Columns;

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
