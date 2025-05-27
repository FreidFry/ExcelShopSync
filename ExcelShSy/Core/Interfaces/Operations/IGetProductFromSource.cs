using ExcelShSy.Core.Interfaces.Excel;

namespace ExcelShSy.Core.Interfaces.Operations
{
    public interface IGetProductFromSource
    {
        void GetAllProduct(IExcelFile file);
    }
}