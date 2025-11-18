using ExcelShSy.Core.Properties;
using OfficeOpenXml;
using System.Globalization;

namespace ExcelShSy.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for reading and converting values from EPPlus worksheets.
    /// </summary>
    public static class ExcelRangeExtensions
    {
        /// <summary>
        /// Reads an article identifier from the specified cell.
        /// </summary>
        /// <param name="worksheet">The worksheet to read from.</param>
        /// <param name="row">The row index.</param>
        /// <param name="needColumn">The column index.</param>
        /// <returns>The article value or <c>null</c>.</returns>
        public static string? GetArticle(this ExcelWorksheet worksheet, int row, int needColumn) => worksheet.GetString(row, needColumn);
        
        /// <summary>
        /// Reads a string value from the specified cell, resolving merged cell values when necessary.
        /// </summary>
        /// <param name="worksheet">The worksheet to read from.</param>
        /// <param name="row">The row index.</param>
        /// <param name="needColumn">The column index.</param>
        /// <returns>The cell value or <c>null</c>.</returns>
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

        /// <summary>
        /// Reads a decimal value from the specified cell, applying rounding rules.
        /// </summary>
        /// <param name="worksheet">The worksheet to read from.</param>
        /// <param name="row">The row index.</param>
        /// <param name="needColumn">The column index.</param>
        /// <returns>The parsed decimal value or <c>null</c>.</returns>
        public static decimal? GetDecimal(this ExcelWorksheet worksheet, int row, int needColumn)
        {
                var stringValue = worksheet.GetString(row, needColumn);
                if (string.IsNullOrEmpty(stringValue)) return null;

               return GetDecimalWithString(stringValue);
        }

        /// <summary>
        /// Parses a string representation of a decimal value and returns the result, optionally rounding according to
        /// application settings.
        /// </summary>
        /// <remarks>If <see cref="ProductProcessingOptions.ShouldRoundPrices"/> is <see
        /// langword="true"/>, the result is rounded to zero decimal places; otherwise, it is rounded to two decimal
        /// places. Parsing uses invariant culture and accepts any number style.</remarks>
        /// <param name="decimalString">The string containing the decimal value to parse. The string should use invariant culture formatting.</param>
        /// <returns>A decimal value parsed from the input string, rounded according to application settings; or null if parsing
        /// fails.</returns>
        internal static decimal? GetDecimalWithString(string decimalString) => GetDecimalWithString(decimalString, ProductProcessingOptions.ShouldRoundPrices);

        /// <summary>
        /// Parses a string representation of a decimal value and returns the result, optionally rounding according to
        /// application settings.
        /// </summary>
        /// <remarks>If manualRound is <see
        /// langword="true"/>, the result is rounded to zero decimal places; otherwise, it is rounded to two decimal
        /// places. Parsing uses invariant culture and accepts any number style.</remarks>
        /// <param name="decimalString">The string containing the decimal value to parse. The string should use invariant culture formatting.</param>
        /// <param name="manualRound">If set to <c>true</c>, rounds to zero decimal places; otherwise, rounds to two decimal places.</param>
        /// <returns>A decimal value parsed from the input string, rounded according to application settings; or null if parsing
        /// fails.</returns>
        internal static decimal? GetDecimalWithString(string decimalString, bool manualRound)
        {
            try
            {
                var normalizedValue = decimalString.Replace(',', '.');
                decimal.TryParse(normalizedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var value);
                if (value < 0) return null;

                if (manualRound) return RoundDecimal(value, 0);
                return RoundDecimal(value, 2);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Reads a discount value from the specified cell.
        /// </summary>
        /// <param name="worksheet">The worksheet to read from.</param>
        /// <param name="row">The row index.</param>
        /// <param name="needColumn">The column index.</param>
        /// <returns>The parsed discount value or <c>null</c>.</returns>
        public static decimal? GetDiscount(this ExcelWorksheet worksheet, int row, int needColumn)
        {
            try
            {
                var stringValue = worksheet.GetString(row, needColumn);
                if (string.IsNullOrEmpty(stringValue)) return null;

                return GetDecimalWithString(stringValue, true);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Reads a quantity value from the specified cell.
        /// </summary>
        /// <param name="worksheet">The worksheet to read from.</param>
        /// <param name="row">The row index.</param>
        /// <param name="needColumn">The column index.</param>
        /// <returns>The parsed quantity value or <c>null</c>.</returns>
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

        /// <summary>
        /// Rounds a decimal value using midpoint rounding away from zero.
        /// </summary>
        /// <param name="value">The value to round.</param>
        /// <param name="numbersAfterDot">The number of decimal places to keep.</param>
        /// <returns>The rounded value.</returns>
        public static decimal RoundDecimal(decimal value, int numbersAfterDot)
        {
            return Math.Round(value, numbersAfterDot, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Reads a date value from the specified cell using the provided format.
        /// </summary>
        /// <param name="worksheet">The worksheet to read from.</param>
        /// <param name="row">The row index.</param>
        /// <param name="needColumn">The column index.</param>
        /// <param name="format">The expected date format.</param>
        /// <returns>The parsed date or <c>null</c>.</returns>
        public static DateTime? GetDate(this ExcelWorksheet worksheet, int row, int needColumn, string? format)
        {
            try
            {
                var value = worksheet.GetString(row, needColumn);
                if (string.IsNullOrWhiteSpace(value) && string.IsNullOrWhiteSpace(format)) return null;
                if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var date))
                    return date;
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
