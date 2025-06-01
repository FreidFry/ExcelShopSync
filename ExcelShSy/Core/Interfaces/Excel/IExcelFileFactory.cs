
namespace ExcelShSy.Core.Interfaces.Excel
{
    public interface IExcelFileFactory
    {
        IExcelFile Create(string path);
    }
}
