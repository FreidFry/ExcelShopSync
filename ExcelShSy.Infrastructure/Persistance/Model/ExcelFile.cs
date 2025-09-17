using ExcelShSy.Core.Interfaces.Excel;

using OfficeOpenXml;

using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

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

        public async Task ShowFileDetails()
        {
            for (int page = 0; page < SheetList.Count; page++)
            {
                var response = SheetList[page].ShowPageDetails();
                var msgBox = MessageBoxManager.GetMessageBoxStandard($"{FileName} ({ShopName}) {page+1}/{SheetList.Count}", response, ButtonEnum.OkCancel);

                var result = await msgBox.ShowAsync();
                if (result == ButtonResult.Ok) continue; //show next message
                
                return; //stop show message
            }
        }
    }
}
