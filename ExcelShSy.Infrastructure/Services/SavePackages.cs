using Avalonia.Media;
using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using ReactiveUI;

namespace ExcelShSy.Infrastructure.Services
{
    [Task("SavePackages")]
    public class SavePackages(IFileStorage fileStorage, IAppSettings appSettings, ILocalizationService localizationService) : IExecuteOperation
    {
        public List<string> Errors { get; } = [];
        private readonly string _errorMsg = localizationService.GetErrorString("CloseWarningText");
        private readonly string _errorTitle = localizationService.GetErrorString("CloseWarning");

        public async Task Execute()
        {
            if (appSettings.CreateNewFileWhileSave)
            {
                await Save(file => file.ExcelPackage.SaveAs(GetNewPath(file.FileLocation)));
                return;
            }

            await Save(file => file.ExcelPackage.Save());
        }

        private async Task Save(Action<IExcelFile> action)
        {
            foreach (var file in fileStorage.Target)
            {
                var formattedText = string.Format(_errorMsg, file.FileName);
                var success = false;

                while (!success)
                {
                    try
                    {
                        action(file);
                        success = true;
                    }
                    catch
                    {
                        await MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(_errorTitle, formattedText).ShowWindowAsync();
                    }
                }
            }
        }

        private static string GetNewPath(string oldPath)
        {
            var extension = Path.GetExtension(oldPath);
            var path = Path.GetDirectoryName(oldPath);
            var name = Path.GetFileNameWithoutExtension(oldPath);
            return path == null ? $"{name}New{extension}" : Path.Combine(path, $"{name}New{extension}");
        }
    }
}
