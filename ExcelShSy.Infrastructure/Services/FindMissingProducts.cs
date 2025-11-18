using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.DataBase;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Core.Properties;

namespace ExcelShSy.Infrastructure.Services
{
    /// <summary>
    /// Generates a report listing target products that are missing from the fetched data set.
    /// </summary>
    [Task(nameof(ProductProcessingOptions.ShouldFindMissingProducts))]
    public class FindMissingProducts(IProductStorage dataProduct, IFileStorage fileStorage, IDatabaseSearcher searcher, ILocalizationService localizationService)
        : IExecuteOperation
    {
        private FileStream? _fileStream;
        private StreamWriter? _writer;

        /// <summary>
        /// Holds the shop name for the file currently being analyzed.
        /// </summary>
        private string? _shopName = string.Empty;
        
        /// <summary>
        /// Gets the list of errors generated during processing.
        /// </summary>
        public List<string> Errors { get; } = [];


        /// <summary>
        /// Creates a report file on the user's desktop and scans all target files for missing products.
        /// </summary>
        public async Task Execute()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "missing.txt");
            _fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            _writer = new StreamWriter(_fileStream, leaveOpen: true);
            foreach (var file in fileStorage.Target)
            {
                _writer.WriteLine(CenterText(file.FileName));
                _shopName = file.ShopName;
                ProcessFile(file);
            }
            _writer.Flush();
            _writer.Dispose();
            _fileStream.Dispose();
        }

        /// <summary>
        /// Processes each worksheet in the specified file and writes missing product information.
        /// </summary>
        /// <param name="file">The file to inspect.</param>
        private void ProcessFile(IExcelFile file)
        {
            if (file.SheetList == null)
            {
                var msg = localizationService.GetErrorString("ErrorNoSheets");
                var formatted = string.Format(msg, file.FileName);
                Errors.Add(formatted);
                return;
            }
            foreach (var page in file.SheetList)
            {
                _writer!.WriteLine(CenterText(page.SheetName));
                OperationWrapper.Try(() => ProcessPage(page), Errors, file.FileName);
            }
        }

        /// <summary>
        /// Collects rows with missing products and writes results to the report.
        /// </summary>
        /// <param name="page">The worksheet abstraction to analyze.</param>
        private void ProcessPage(IExcelSheet page)
        {
            var worksheet = page.Worksheet;

            var articleCol = page.InitialHeadersTuple();

            if (articleCol == 0) return;

            var missing = new List<string>();
            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var localArticle = worksheet.GetArticle(row, articleCol);
                if (localArticle == null)
                {
                    missing.Add($"row: {row} - empty product");
                    continue;
                }
                
                var article = searcher.SearchProduct(_shopName, localArticle);
                
                if (!dataProduct.Articles.Contains(article)) missing.Add($"row: {row} - {localArticle} not found");
            }
            _writer!.WriteLine(string.Join(Environment.NewLine, missing));
            var totalRow = worksheet.Dimension.Rows - 1;
            _writer.WriteLine(CenterText($"{missing.Count}/{totalRow} not founded"));
        }

        /// <summary>
        /// Creates a centered header string padded with filler characters.
        /// </summary>
        /// <param name="text">The text to center.</param>
        /// <param name="totalWidth">The total width of the resulting string.</param>
        /// <param name="filler">The character used for padding.</param>
        /// <returns>The centered text.</returns>
        private static string CenterText(string text, int totalWidth = 80, char filler = '-')
        {
            var padding = totalWidth - text.Length;
            var padLeft = padding / 2;
            var padRight = padding - padLeft;
            return new string(filler, padLeft) + text + new string(filler, padRight);
        }
    }
}
