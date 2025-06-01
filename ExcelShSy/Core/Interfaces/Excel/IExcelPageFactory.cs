using OfficeOpenXml;

namespace ExcelShSy.Core.Interfaces.Excel
{
    public interface IExcelPageFactory
    {
        IExcelPage Create(ExcelWorksheet worksheet);
    }
}
