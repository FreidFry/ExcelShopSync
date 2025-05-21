using OfficeOpenXml;

namespace ExcelShSy.Core.Extensions
{
    public static class ExcelRangeExtensions
    {
        public static string? GetArticle(this ExcelWorksheet worksheet, int article, int needColumn) =>
            worksheet.GetValue(article, needColumn)?.ToString();

        public static decimal? GetDecimal(this ExcelWorksheet worksheet, int article, int needColumn)
        {
            try
            {
                var value = worksheet.GetValue(article, needColumn)?.ToString();
                if (string.IsNullOrEmpty(value)) return null;

                value = value.Trim().Replace(',', '.');
                var decimalValue = decimal.Parse(value);
                return decimalValue;
            }
            catch
            {
                return null;
            }
        }

        public static decimal? GetAvailability(this ExcelWorksheet worksheet, int article, int needColumn) =>
            worksheet.GetDecimal(article, needColumn);
    }
}
