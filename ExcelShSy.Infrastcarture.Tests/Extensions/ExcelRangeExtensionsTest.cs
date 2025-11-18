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
    }
}