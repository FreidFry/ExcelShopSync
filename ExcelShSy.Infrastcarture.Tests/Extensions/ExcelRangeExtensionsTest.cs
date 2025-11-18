using ExcelShSy.Core.Properties;

namespace ExcelShSy.Tests.Extensions
{
    public class ExcelRangeExtensionsTest
    {
        [Theory]
        [InlineData("10000", false, 10000)]
        [InlineData("10000,45623", false, 10000.46)]
        [InlineData("10000.45623", true, 10000)]
        [InlineData("10000.45623 ", true, 10000)]
        public void GetDecimalWithString(string input, bool roundPrice, decimal expected)
        {
            ProductProcessingOptions.ShouldRoundPrices = roundPrice;
            var result = Infrastructure.Extensions.ExcelRangeExtensions.GetDecimalWithString(input);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("-10000.45623 ", true, null)]
        [InlineData("-10000.45623 ", false, null)]
        public void GetDecimalWithString_NullReturn(string input, bool roundPrice, decimal? expected)
        {
            ProductProcessingOptions.ShouldRoundPrices = roundPrice;
            var result = Infrastructure.Extensions.ExcelRangeExtensions.GetDecimalWithString(input);

            Assert.Equal(expected, result);
        }
        [Theory]
        [InlineData("2001 23 11 15 06 11", "yyyy dd MM HH mm ss", 2001, 11, 23, 15, 06, 11)]
        [InlineData("2001.23.11.15.06.11", "yyyy.dd.MM.HH.mm.ss", 2001, 11, 23, 15, 06, 11)]

        public void GetData(string dataString, string format, int year, int month, int day, int hour, int minute, int second)
        {
            var expected = new DateTime(year, month, day, hour, minute, second);
            var result = Infrastructure.Extensions.ExcelRangeExtensions.DateFromString(dataString, format);
            Assert.Equal(expected, result);
        }
    }
}