using ExcelShSy.Core.Interfaces.Excel;

using OfficeOpenXml;

using System.IO;
using System.Windows;

namespace ExcelShSy.Infrastructure.Persistance.Model
{
    public class ExcelFile : IExcelFile
    {
        public string FileLocation { get; set; }
        public string FileName { get; set; }
        public string ShopName { get; set; }
        public string Language { get; set; }
        public ExcelPackage ExcelPackage { get; set; }
        public List<IExcelSheet> SheetList { get; set; }

        public ExcelFile(string path)
        {
            FileLocation = path;
            FileName = Path.GetFileName(path);
            ExcelPackage = new ExcelPackage(path);
        }

        public void ShowFileDetails()
        {
            for (int page = 0; page < SheetList.Count; page++)
            {
                var response = SheetList[page].ShowPageDetails();
                var s = MessageBox.Show(response, $"{FileName} ({ShopName}) {page+1}/{SheetList.Count}", MessageBoxButton.OKCancel);
                if (s == MessageBoxResult.OK) continue; //show next message

                return; //stop show message
            }
        }
    }
}
