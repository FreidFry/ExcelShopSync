using ExcelShSy.Core.Interfaces.Excel;

using OfficeOpenXml;

using System.IO;

namespace ExcelShSy.Infrastructure.Persistance.Model
{
    public class ExcelFile : IExcelFile
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ShopName { get; set; }
        public string Language { get; set; }
        public ExcelPackage ExcelPackage { get; set; }
        public List<IExcelPage> Pages { get; set; }

        public ExcelFile(string path)
        {
            FilePath = path;
            FileName = Path.GetFileName(path);
            ExcelPackage = new ExcelPackage(path);
        }

        public void ShowInfo()
        {
            foreach (var page in Pages)
                if (page.ShowInfo() != true) return; //if press ok => continue, else => stop
        }
    }
}
