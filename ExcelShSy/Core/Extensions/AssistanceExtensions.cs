using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Infrastracture.Persistance.DefaultValues;

using OfficeOpenXml;
using OfficeOpenXml.Style;

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows;

namespace ExcelShSy.Core.Extensions
{
    public static class AssistanceExtensions
    {
        public static (int articleColumn, int neededColumn) InitialHeadersTuple(this IExcelPage page, string column)
        {
            if (page?.Headers == null || page.Headers.IsNullOrEmpty() || !page.Headers.ContainsKey(column)) return (0, 0);

            return (page.Headers[ColumnConstants.Article], page.Headers[column]);
        }

        public static bool AnyIsNullOrEmpty([NotNullWhen(false)] this (int, int) tuple) => tuple.Item1 is 0 || tuple.Item2 is 0;

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

        public static void ChangeCellColor(this ExcelWorksheet worksheet, int row, int column)
        {
            worksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;

            if (worksheet.Cells[row, column].Style.Fill.BackgroundColor.Rgb == "FFFFFF00")
                worksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(Color.Green);
            else
                worksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
        }

        public static void Warning(bool check, string message)
        {
            if (check)
            {
                var result = MessageBox.Show(
                $"{message}",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
        }
    }
}
