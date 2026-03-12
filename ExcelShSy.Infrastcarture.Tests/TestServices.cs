using ExcelShSy.Tests;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;

namespace ExcelShSy.Tests
{
    internal static class TestServices
    {
        public static ServiceProvider Build()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Freid4");
            var services = new ServiceCollection();
            services.AddDependencyInjection();
            return services.BuildServiceProvider();
        }
    }
}
