using ExcelShSy.Core.Interfaces.Excel;
using Microsoft.Extensions.DependencyInjection;

namespace ExcelShSy.Tests.Factories
{
    public class ExcelFileFactoryTest
    {
        [Theory]
        [InlineData("Files\\shop1.xlsx")]
        public void Create_ValidFile_ReturnsExcelPackage(string filePath)
        {
            // Arrange
            var provider = TestServices.Build();
            var fullpath = Path.Combine(Environment.CurrentDirectory, filePath);

            var factory = provider.GetRequiredService<IExcelFileFactory>();

            // Act
            var package = factory.Create(fullpath);
            // Assert
            Assert.NotNull(package);
            Assert.True(package.ExcelPackage.Workbook.Worksheets.Count > 0);
        }
    }
}