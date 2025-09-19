using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ExcelShSy.Infrastructure.Extensions;
using ExcelShSy.Core.Properties;

namespace ExcelShSy.Infrastructure.Services
{
    [Task(nameof(ProductProcessingOptions.ShouldFindMissingProducts))]
    public class FindMissingProducts : IExecuteOperation
    {
        private readonly IProductStorage _dataProduct;
        private readonly IFileStorage _fileStorage;
        private FileStream _fileStream;
        private StreamWriter _writer;

        public List<string> Errors { get; } = [];

        public FindMissingProducts(IProductStorage dataProduct, IFileStorage fileStorage)
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;
        }


        public void Execute()
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "missing.txt");
            _fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            _writer = new StreamWriter(_fileStream, leaveOpen: true);
            foreach (var file in _fileStorage.Target)
            {
                _writer.WriteLine(CenterText(file.FileName));
                ProcessFile(file);
            }
            _writer.Flush();
            _writer.Dispose();
            _fileStream.Dispose();
        }

        void ProcessFile(IExcelFile file)
        {
            foreach (var page in file.SheetList)
            {
                _writer.WriteLine(CenterText(page.SheetName));
                OperationWraper.Try(() => ProcessPage(page), Errors, file.FileName);
            }
        }

        void ProcessPage(IExcelSheet page)
        {
            var worksheet = page.Worksheet;

            var articleCol = page.InitialHeadersTuple();

            if (articleCol == 0) return;

            var missing = new List<string>();
            foreach (var row in worksheet.GetFullRowRangeWithoutFirstRow())
            {
                var article = worksheet.GetArticle(row, articleCol);

                if (article == null)
                {
                    missing.Add($"row: {row} - empty product");
                    continue;
                }
                if (!_dataProduct.Articles.Contains(article)) missing.Add($"row: {row} - {article} not found");
            }
            _writer.WriteLine(string.Join(Environment.NewLine, missing));
            int totalRow = worksheet.Dimension.Rows - 1;
            _writer.WriteLine(CenterText($"{missing.Count}/{totalRow} not founded"));
        }

        static string CenterText(string text, int totalWidth = 80, char filler = '-')
        {
            int padding = totalWidth - text.Length;
            int padLeft = padding / 2;
            int padRight = padding - padLeft;
            return new string(filler, padLeft) + text + new string(filler, padRight);
        }
    }
}
