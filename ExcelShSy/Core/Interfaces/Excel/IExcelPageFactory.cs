using ExcelShSy.Infrastracture.Persistance.Model;
using OfficeOpenXml;

namespace ExcelShSy.Core.Interfaces.Excel
{
    public interface IExcelPageFactory
    {
        ExcelPage Create(ExcelWorksheet worksheet);
    }
}
