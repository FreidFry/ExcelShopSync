using ExcelShSy.Properties;

using OfficeOpenXml;

namespace ExcelShSy.Core.Extensions
{
    public static class ExcelRangeExtensions
    {
        public static string? GetArticle(this ExcelWorksheet worksheet, int row, int needColumn) =>
            worksheet.GetValue(row, needColumn)?.ToString();

        public static string? GetString(this ExcelWorksheet worksheet, int row, int needColumn) => worksheet.GetArticle(row, needColumn);

        public static decimal? GetDecimal(this ExcelWorksheet worksheet, int row, int needColumn)
        {
            try
            {
                var value = worksheet.GetValue(row, needColumn)?.ToString();
                if (string.IsNullOrEmpty(value)) return null;

                value = value.Trim().Replace(',', '.');
                var decimalValue = Math.Round(decimal.Parse(value), 2, MidpointRounding.AwayFromZero);
                if (GlobalSettings.IsRound) decimalValue = Math.Ceiling(decimalValue);
                return decimalValue;
            }
            catch
            {
                return null;
            }
        }

        public static decimal? GetAvailability(this ExcelWorksheet worksheet, int row, int needColumn)
        {
            try
            {
                var value = worksheet.GetValue(row, needColumn)?.ToString();
                if (string.IsNullOrEmpty(value)) return null;

                value = value.Trim().Replace(',', '.');
                var decimalValue = Math.Round(decimal.Parse(value), 2, MidpointRounding.AwayFromZero);
                return decimalValue;
            }
            catch
            {
                return null;
            }
        }
    }
}
