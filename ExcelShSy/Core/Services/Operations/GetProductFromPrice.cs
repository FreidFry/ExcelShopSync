using ExcelShSy.Core.Extensions;
using ExcelShSy.Core.Interfaces;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using OfficeOpenXml;

namespace ExcelShSy.Core.Services.Operations
{
    public class GetProductFromPrice : IGetProductFromPrice
    {
        readonly IDataProduct _dataProduct;
        readonly IFileStorage _fileStorage;

        public GetProductFromPrice(IDataProduct dataProduct, IFileStorage fileStorage)
        {
            _dataProduct = dataProduct;
            _fileStorage = fileStorage;
        }

        public void GetProducts()
        {
            foreach (var file in _fileStorage.Source) ProcessFile(file);
        }

        void ProcessFile(IExcelFile file)
        {
            foreach (var page in file.Pages) ProcessPage(page);
        }



        void ProcessPage(IExcelPage page)
        {
            var worksheet = page.ExcelWorksheet;
            var header = InitHeaders();
            var headerComplect = InitHeaders();

            foreach (var row in page.GetFullRowRange())
            {
                bool HeaderRow = false;

                var range = worksheet.GetRowValueColumnMap(row);
                if (IsValidRange(range)) continue;

                var headers = TryGetHeaders(range, row);
                if(headers != null && headers?.ArticleCol != 0)
                {
                    header = headers;
                    HeaderRow = true;
                }
                var headersComplect = TryGetHeadersComplect(range, row);
                if (headersComplect != null && headersComplect?.ArticleCol != 0)
                {
                    headerComplect = headersComplect;
                    HeaderRow = true;
                }

                if (HeaderRow) continue;

                ProcessRow(worksheet, (header, headerComplect), row);
            }
        }

        static HeaderMap InitHeaders()
        {
            return new HeaderMap();
        }

        static bool IsValidRange(Dictionary<string, int>? range)
        {
            return range != null && range.Count <= 2;
        }

        static HeaderMap? TryGetHeaders(Dictionary<string, int>? range, int row)
        {
            return range.GetIndefyPriceHeader(row) ?? null;
        }

        static HeaderMap? TryGetHeadersComplect(Dictionary<string, int>? range, int row)
        {
            return range.GetIndefyPriceHeaderComplect(row) ?? null;
        }

        void ProcessRow(ExcelWorksheet worksheet, (HeaderMap? Regular, HeaderMap? Complect) headerMaps, int row)
        {
            ProcessProductMap(worksheet, headerMaps.Regular, row);
            ProcessProductMap(worksheet, headerMaps.Complect, row);
        }

        void ProcessProductMap(ExcelWorksheet worksheet, HeaderMap? map, int row)
        {
            if (map == null || map.ArticleCol == 0) return;

            var article = worksheet.GetArticle(row, (int)map.ArticleCol);
            if (article == null) return;

            ProcessPrice(worksheet, map, row, article);
            ProcessAvailability(worksheet, map, row, article);
        }

        void ProcessPrice(ExcelWorksheet worksheet, HeaderMap map, int row, string article)
        {
            if (map.PriceCol == 0) return;

            var price = worksheet.GetDecimal(row, (int)map.PriceCol);
            if (price.HasValue)
            {
                _dataProduct.AddProductPrice(article, price.Value);
            }
        }

        void ProcessAvailability(ExcelWorksheet worksheet, HeaderMap map, int row, string article)
        {
            if (map.AvailabilityCol == 0) return;

            var availability = worksheet.GetString(row, (int)map.AvailabilityCol);
            if (availability != null)
            {
                _dataProduct.AddProductAvailability(article, availability);
            }
        }
    }
}
