using ExcelShSy.Core.Enums;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using OfficeOpenXml;

namespace ExcelShSy.Infrastructure.Persistence.Model
{
    /// <summary>
    /// Concrete implementation of <see cref="IExcelFile"/> backed by EPPlus.
    /// </summary>
    public class ExcelFile : IExcelFile
    {
        private readonly IMessages<IMsBox<ButtonResult>> _messages;
        /// <inheritdoc />
        public string FileLocation { get; set; }
        /// <inheritdoc />
        public string FileName { get; set; }
        /// <inheritdoc />
        public string? ShopName { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string? Language { get; set; }
        /// <inheritdoc />
        public ExcelPackage ExcelPackage { get; set; }
        /// <inheritdoc />
        public List<IExcelSheet>? SheetList { get; set; }

        public ExcelFile(IMessages<IMsBox<ButtonResult>> messages, string path)
        {
            _messages = messages;
            FileLocation = path;
            FileName = Path.GetFileName(path);
            ExcelPackage = new ExcelPackage(path);
        }

        /// <inheritdoc />
        public async Task ShowFileDetails()
        {
            for (var page = 0; page < (SheetList?.Count ?? 0); page++)
            {
                var response = SheetList![page].ShowPageDetails();
                var msgBox = _messages.GetMessageBoxStandard(FileName, $"{FileName} ({ShopName}) {page + 1}/{SheetList.Count}\n{response}", MyButtonEnum.OkCancel);

                var result = await msgBox.ShowAsync();
                if (result == ButtonResult.Ok) continue; //show next message
                
                return; //stop show message
            }
        }
    }
}
