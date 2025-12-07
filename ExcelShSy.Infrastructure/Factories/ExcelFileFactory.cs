using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Shop;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Infrastructure.Persistence.Model;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using OfficeOpenXml;

namespace ExcelShSy.Infrastructure.Factories
{
    /// <summary>
    /// Creates <see cref="IExcelFile"/> instances and infers metadata such as shop and language.
    /// </summary>
    public class ExcelFileFactory(IMessagesService<IMsBox<ButtonResult>> messages, IExcelPageFactory excelPageFactory, IShopStorage shopStorage)
        : IExcelFileFactory
    {
        /// <inheritdoc />
        public IExcelFile Create(string path)
        {
            var file = new ExcelFile(messages, path);

            file.SheetList = GetPages(file.ExcelPackage);

            file.ShopName = IdentifyShop(file.SheetList);
            file.Language = LanguageDetect(file.SheetList);

            return file;
        }

        /// <summary>
        /// Builds page abstractions for all worksheets in the package.
        /// </summary>
        /// <param name="package">The Excel package to inspect.</param>
        /// <returns>The list of sheet abstractions.</returns>
        private List<IExcelSheet> GetPages(ExcelPackage package)
        {
            ExcelWorkbook workbook = package.Workbook;
            if (workbook == null)
                return [];
            List<IExcelSheet> pages = [];
            pages.AddRange(workbook.Worksheets.Select(excelPageFactory.Create));
            return pages;
        }

        /// <summary>
        /// Attempts to identify the shop for the provided pages by majority vote.
        /// </summary>
        /// <param name="pages">The pages to evaluate.</param>
        /// <returns>The inferred shop name.</returns>
        private string? IdentifyShop(List<IExcelSheet> pages)
        {
            List<string> shops = [];
            foreach (var page in pages)
            {
                shops.Add(page.GetShop(shopStorage));
                if (shops.Count > 6) break;
            }

            var thisShop = shops.GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?
                .Key;

            return thisShop;
        }

        /// <summary>
        /// Detects the predominant language across the provided pages.
        /// </summary>
        /// <param name="pages">The pages to evaluate.</param>
        /// <returns>The inferred language identifier.</returns>
        private static string? LanguageDetect(List<IExcelSheet> pages)
        {
            List<string> languages = [];
            foreach (var page in pages)
            {
                languages.Add(page.GetLanguage());
                if (languages.Count > 15) break;
            }

            var thisLanguage = languages.GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?
                .Key;

            return thisLanguage;
        }

    }
}
