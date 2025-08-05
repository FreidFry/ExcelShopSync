using ExcelShSy.Core.Interfaces.Excel;

namespace ExcelShSy.Core.Interfaces.Operations
{
    public interface IFetchProductBase
    {
        void FetchAllProducts(IExcelFile file);
    }
}