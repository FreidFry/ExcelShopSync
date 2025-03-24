using System.Drawing;
using System.Globalization;
using System.Windows;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ExcelShopSync.Services.Base
{
    class AssistanceMethods
    {
        public static void FillCell(ExcelWorksheet worksheet, int row, int column, object value)
        {
            worksheet.Cells[row, column].Value = PrepareExcelValue<object>(value);
            worksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;

            if (worksheet.Cells[row, column].Style.Fill.BackgroundColor.Rgb == "FFFFFF00")
            {
                worksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(Color.Green);
            }
            else
            {
                worksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            }
        }

        public static T PrepareExcelValue<T>(object value, string decimalFormat = "0.##", bool useEnglishCulture = false)
        {
            var culture = useEnglishCulture ? new CultureInfo("ru-RU") : CultureInfo.InvariantCulture;

            if (value is string strValue)
            {
                strValue = strValue.Replace(" ", "").Replace(',', '.').Trim();

                if (typeof(T) == typeof(double))
                {
                    if (double.TryParse(strValue, NumberStyles.Any, culture, out double doubleValue))
                    {
                        return (T)(object)doubleValue;
                    }
                }
                else if (typeof(T) == typeof(decimal))
                {
                    if (decimal.TryParse(strValue, NumberStyles.Any, culture, out decimal decimalValue))
                    {
                        return (T)(object)decimalValue;
                    }
                }
                else if (typeof(T) == typeof(int))
                {
                    if (int.TryParse(strValue, out int intValue))
                    {
                        return (T)(object)intValue;
                    }
                }
                else if (typeof(T) == typeof(DateTime))
                {
                    if (DateTime.TryParse(strValue, out DateTime dateValue))
                    {
                        return (T)(object)dateValue;
                    }
                }
            }

            return (T)value;
        }



        public static void warning(bool check, string message)
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
