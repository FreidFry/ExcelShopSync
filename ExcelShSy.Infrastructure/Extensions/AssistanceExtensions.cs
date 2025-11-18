using ExcelShSy.Core.Exceptions;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Infrastructure.Persistence.DefaultValues;
using OfficeOpenXml;
using OfficeOpenXml.Style;

using Color = System.Drawing.Color;


namespace ExcelShSy.Infrastructure.Extensions
{
    /// <summary>
    /// Helper extensions for working with Excel sheets and writing values while tracking changes.
    /// </summary>
    public static class AssistanceExtensions
    {
        /// <summary>
        /// Retrieves the article and target columns for the specified logical column name.
        /// </summary>
        /// <param name="page">The sheet containing the mappings.</param>
        /// <param name="columnName">The logical column name to locate.</param>
        /// <returns>A tuple of article and target column indexes.</returns>
        public static (int articleColumn, int neededColumn) InitialHeadersTuple(this IExcelSheet page, string columnName)
        {
            if (page.MappedHeaders == null || page.MappedHeaders.IsNullOrEmpty() || !page.MappedHeaders.ContainsKey(columnName)) return (0, 0);

            var article = page.FindColumnInHeaders(ColumnConstants.Article);
            var needColumn = page.FindColumnInHeaders(columnName);

            return (article, needColumn);
        }

        /// <summary>
        /// Retrieves the article column index from the sheet headers.
        /// </summary>
        /// <param name="page">The sheet containing the mappings.</param>
        /// <returns>The article column index or zero when not found.</returns>
        public static int InitialHeadersTuple(this IExcelSheet page)
        {
            if (page.MappedHeaders == null || page.MappedHeaders.IsNullOrEmpty()) return 0;

            return page.FindColumnInHeaders(ColumnConstants.Article);
        }

        /// <summary>
        /// Retrieves the target column index for the specified logical column name.
        /// </summary>
        /// <param name="page">The sheet containing the mappings.</param>
        /// <param name="columnName">The logical column name to locate.</param>
        /// <returns>The column index or zero when not found.</returns>
        public static int InitialNeedColumn(this IExcelSheet page, string columnName)
        {
            if (page.MappedHeaders == null || page.MappedHeaders.IsNullOrEmpty()) return 0;

            return page.FindColumnInHeaders(columnName);
        }

        /// <summary>
        /// Finds the column index for the specified logical column name, throwing if not present.
        /// </summary>
        /// <param name="page">The sheet containing the mappings.</param>
        /// <param name="columnName">The logical column name to locate.</param>
        /// <returns>The column index.</returns>
        /// <exception cref="ShopDataException">Thrown when the column cannot be found.</exception>
        private static int FindColumnInHeaders(this IExcelSheet page, string columnName)
        {
            if (page.MappedHeaders == null || page.MappedHeaders.IsNullOrEmpty())
                throw new ShopDataException($"page \"{page.SheetName}\" - \"{columnName}\" not found! Please check your file.");
            page.MappedHeaders.TryGetValue(columnName, out var value);
            return value;
        }

        /// <summary>
        /// Determines whether either element in the tuple is zero.
        /// </summary>
        /// <param name="tuple">The tuple to inspect.</param>
        /// <returns><c>true</c> if any value is zero; otherwise, <c>false</c>.</returns>
        public static bool AnyIsNullOrEmpty(this (int, int) tuple) => tuple.Item1 is 0 || tuple.Item2 is 0;

        /// <summary>
        /// Writes a string value to the specified cell, highlighting changes.
        /// </summary>
        /// <param name="worksheet">The worksheet to modify.</param>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteCell(this ExcelWorksheet worksheet, int row, int column, string value)
        {
            var currentValue = worksheet.GetValue(row, column)?.ToString();
            if (currentValue != value)
            {
                worksheet.Cells[row, column].Value = value;
                worksheet.ChangeCellColor(row, column);
            }
        }

        /// <summary>
        /// Writes a decimal value to the specified cell, highlighting changes.
        /// </summary>
        /// <param name="worksheet">The worksheet to modify.</param>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteCell(this ExcelWorksheet worksheet, int row, int column, decimal value)
        {
            var currentValue = worksheet.GetDecimal(row, column);
            if (currentValue != value)
            {
                worksheet.Cells[row, column].Value = value;
                worksheet.ChangeCellColor(row, column);
            }
        }

        /// <summary>
        /// Toggles the background color of the cell to indicate it has been modified.
        /// </summary>
        /// <param name="worksheet">The worksheet containing the cell.</param>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
        private static void ChangeCellColor(this ExcelWorksheet worksheet, int row, int column)
        {
            worksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;

            worksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(
                worksheet.Cells[row, column].Style.Fill.BackgroundColor.Rgb == "FFFFFF00" ? Color.Green : Color.Yellow);
        }
    }
}
