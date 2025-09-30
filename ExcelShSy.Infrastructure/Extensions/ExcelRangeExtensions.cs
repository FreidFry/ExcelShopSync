using OfficeOpenXml;
using System.Globalization;
using ExcelShSy.Core.Properties;
using ExcelShSy.LocalDataBaseModule.Data;

namespace ExcelShSy.Infrastructure.Extensions
{
    public static class ExcelRangeExtensions
    {
        public static string? GetArticle(this ExcelWorksheet worksheet, int row, int needColumn) => worksheet.GetString(row, needColumn);
        
        public static string? GetString(this ExcelWorksheet worksheet, int row, int needColumn)
        {
            var cell = worksheet.Cells[row, needColumn];

            if (cell.Merge)
            {
                string mergedAddress = worksheet.MergedCells[cell.Start.Row, cell.Start.Column];

                var firstCell = worksheet.Cells[new ExcelAddress(mergedAddress).Start.Row, new ExcelAddress(mergedAddress).Start.Column];
                return firstCell.Value?.ToString();
            }
            return cell.Value?.ToString();
        }

        public static decimal? GetDecimal(this ExcelWorksheet worksheet, int row, int needColumn)
        {
            try
            {
                var stringValue = worksheet.GetString(row, needColumn);
                if (string.IsNullOrEmpty(stringValue)) return null;

                var normalizedValue = stringValue.Replace(',', '.');
                decimal.TryParse(normalizedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var value);
                
                if (ProductProcessingOptions.ShouldRoundPrices) return RoundDecimal(value, 0);
                return RoundDecimal(value, 2);
            }
            catch
            {
                return null;
            }
        }

        public static decimal? GetDiscount(this ExcelWorksheet worksheet, int row, int needColumn)
        {
            try
            {
                var stringValue = worksheet.GetString(row, needColumn);
                if (string.IsNullOrEmpty(stringValue)) return null;

                var normalizedValue = stringValue.Replace(',', '.');
                decimal.TryParse(normalizedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var value);

                return RoundDecimal(value, 2);
            }
            catch
            {
                return null;
            }
        }

        public static decimal? GetQuantity(this ExcelWorksheet worksheet, int row, int needColumn)
        {
            try
            {
                var stringValue = worksheet.GetString(row, needColumn);
                if (string.IsNullOrEmpty(stringValue)) return null;
                var normalizedValue = stringValue.Replace(',', '.');
                decimal.TryParse(normalizedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var value);
                return RoundDecimal(value, 2);
            }
            catch
            {
                return null;
            }
        }

        public static decimal RoundDecimal(decimal value, int NumbersAfterDot)
        {
            return Math.Round(value, NumbersAfterDot, MidpointRounding.AwayFromZero);
        }

        public static DateOnly? GetDate(this ExcelWorksheet worksheet, int row, int needColumn)
        {
            try
            {
                var value = worksheet.GetString(row, needColumn);
                if (string.IsNullOrEmpty(value)) return null;
                DateOnly.TryParse(value, out var time);
                return time;
            }
            catch
            {
                return null;
            }
        }


    }
}
