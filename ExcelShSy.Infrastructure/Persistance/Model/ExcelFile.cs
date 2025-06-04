using ExcelShSy.Core.Interfaces.Excel;

using OfficeOpenXml;

using System.IO;
using System.Windows;

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
            for (int page = 0; page < Pages.Count; page++)
            {
                var response = Pages[page].ShowInfo();
                var s = MessageBox.Show(response, $"{FileName} ({ShopName}) {page+1}/{Pages.Count}", MessageBoxButton.OKCancel);
                if (s == MessageBoxResult.OK) continue; //show next message

                return; //stop show message
            }
        }
    }
}
