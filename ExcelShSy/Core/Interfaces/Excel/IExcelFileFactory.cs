using ExcelShSy.Infrastracture.Persistance.Model;

namespace ExcelShSy.Core.Interfaces.Excel
{
    public interface IExcelFileFactory
    {
        ExcelFile Create(string path);
    }
}
