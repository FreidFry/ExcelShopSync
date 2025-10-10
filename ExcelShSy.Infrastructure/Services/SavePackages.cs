using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;

namespace ExcelShSy.Infrastructure.Services
{
    [Task("SavePackages")]
    public class SavePackages(IFileStorage fileStorage, IAppSettings appSettings, ILocalizationService localizationService) : IExecuteOperation
    {
        public List<string> Errors { get; } = [];

        public void Execute()
        {
            if (appSettings.CreateNewFileWhileSave)
                foreach (var file in fileStorage.Target)
                    file.ExcelPackage.SaveAs(GetNewPath(file.FileLocation));
            
            var title = localizationService.GetErrorString("CloseWarning");
            var msg = localizationService.GetErrorString("CloseWarningText");
            foreach (var file in fileStorage.Target)
            {
                var formattedText = string.Format(msg, file.FileName);
                var success = true;
                do
                {
                    try
                    {
                        file.ExcelPackage.Save();
                        success = false;
                    }
                    catch
                    {
                        MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(title, formattedText);
                    }
                } while (success);
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
