using ExcelShSy.Core.Attributes;
using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Core.Interfaces.Excel;
using ExcelShSy.Core.Interfaces.Operations;
using ExcelShSy.Core.Interfaces.Storage;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Infrastructure.Services
{
    /// <summary>
    /// Saves updated Excel packages, optionally creating new files to preserve originals.
    /// </summary>
    [Task("SavePackages")]
    public class SavePackages(IFileStorage fileStorage, IAppSettings appSettings, ILocalizationService localizationService, IMessages<IMsBox<ButtonResult>> messages) : IExecuteOperation
    {
        /// <summary>
        /// Gets the list of errors that occurred during save operations.
        /// </summary>
        public List<string> Errors { get; } = [];
        private readonly string _errorMsg = localizationService.GetErrorString("CloseWarningText");
        private readonly string _errorTitle = localizationService.GetErrorString("CloseWarning");

        /// <summary>
        /// Saves each target file according to the configured application settings.
        /// </summary>
        public async Task Execute()
        {
            if (appSettings.CreateNewFileWhileSave)
                await Save(file => file.ExcelPackage.SaveAs(GetNewPath(file.FileLocation)));
            else
                await Save(file => file.ExcelPackage.Save());
        }

        /// <summary>
        /// Executes the provided save action for each target file, retrying if the file is locked.
        /// </summary>
        /// <param name="action">The save action to perform.</param>
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
                        await messages.GetMessageBoxStandard(_errorTitle, formattedText).ShowWindowAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Generates a new file path by appending <c>New</c> to the original file name.
        /// </summary>
        /// <param name="oldPath">The existing file path.</param>
        /// <returns>The newly generated file path.</returns>
        private static string GetNewPath(string oldPath)
        {
            var extension = Path.GetExtension(oldPath);
            var path = Path.GetDirectoryName(oldPath);
            var name = Path.GetFileNameWithoutExtension(oldPath);
            return path == null ? $"{name}New{extension}" : Path.Combine(path, $"{name}New{extension}");
        }
    }
}
