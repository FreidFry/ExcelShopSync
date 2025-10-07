using OfficeOpenXml;

namespace ExcelShSy.Core.Interfaces.Excel
{
    public interface IExcelPageFactory
    {
        IExcelSheet Create(ExcelWorksheet worksheet);
    }
}
