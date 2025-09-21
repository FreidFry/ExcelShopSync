using ExcelShSy.Core.Exeptions;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Infrastructure.Persistance.DefaultValues;

using OfficeOpenXml;
using OfficeOpenXml.Style;

using System.Diagnostics.CodeAnalysis;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.LocalDataBaseModule.Data;
using Color = System.Drawing.Color;


namespace ExcelShSy.Infrastructure.Extensions
{
    public static class AssistanceExtensions
    {
        public static (int articleColumn, int neededColumn) InitialHeadersTuple(this IExcelSheet page, string columnName)
        {
            if (page?.MappedHeaders == null || page.MappedHeaders.IsNullOrEmpty() || !page.MappedHeaders.ContainsKey(columnName)) return (0, 0);

            var article = page.FindColumnInHeaders(ColumnConstants.Article);
            var needColumn = page.FindColumnInHeaders(columnName);

            return (article, needColumn);
        }

        public static int InitialHeadersTuple(this IExcelSheet page)
        {
            if (page?.MappedHeaders == null || page.MappedHeaders.IsNullOrEmpty()) return 0;

            return page.FindColumnInHeaders(ColumnConstants.Article);
        }

        public static int InitialNeedColumn(this IExcelSheet page, string columnName)
        {
            if (page?.MappedHeaders == null || page.MappedHeaders.IsNullOrEmpty()) return 0;

            return page.FindColumnInHeaders(columnName);
        }

        private static int FindColumnInHeaders(this IExcelSheet page, string columnName)
        {
            if (page?.MappedHeaders == null || page.MappedHeaders.IsNullOrEmpty() || !page.MappedHeaders.TryGetValue(columnName, out int value))
                throw new ShopDataException($"page \"{page.SheetName}\" - {columnName} not found! Please check your file.");
            return value;
        }

        public static bool AnyIsNullOrEmpty(this (int, int) tuple) => tuple.Item1 is 0 || tuple.Item2 is 0;

        public static void WriteCell(this ExcelWorksheet worksheet, int row, int column, string value)
        {
            var currentValue = worksheet.GetValue(row, column)?.ToString();
            if (currentValue != value)
            {
                worksheet.Cells[row, column].Value = value;
                worksheet.ChangeCellColor(row, column);
            }
        }

        public static void WriteCell(this ExcelWorksheet worksheet, int row, int column, decimal value)
        {
            var currentValue = worksheet.GetDecimal(row, column);
            if (currentValue != value)
            {
                worksheet.Cells[row, column].Value = value;
                worksheet.ChangeCellColor(row, column);
            }
        }

        public static void WriteCell(this ExcelWorksheet worksheet, int row, int column, DateOnly value)
        {
            var currentValue = worksheet.GetDate(row, column);
            if (currentValue != value)
            {
                worksheet.Cells[row, column].Value = value;
                worksheet.ChangeCellColor(row, column);
            }
        }

        private static void ChangeCellColor(this ExcelWorksheet worksheet, int row, int column)
        {
            worksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;

            worksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(
                worksheet.Cells[row, column].Style.Fill.BackgroundColor.Rgb == "FFFFFF00" ? Color.Green : Color.Yellow);
        }
    }
}
