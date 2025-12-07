using ExcelShSy.Core.Enums;

namespace ExcelShSy.Core.Interfaces.ViewModels
{
    public interface IEditLoadFilesModel
    {
        Task ApplyAsync();
        Task ShowInfoFromPath(string path);
        Task AddFile(FileTagEnum tag);
        Task RemoveFile(FileTagEnum tag);
        Task Cancel();
    }
}