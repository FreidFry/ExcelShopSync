namespace ExcelShSy.Core.Interfaces.Storage
{
    public interface IFileManager
    {
        List<string> TargetPaths { get; set; }
        List<string> SourcePaths { get; set; }
        
        void InitializeAllFiles();
        void ClearAfterComplete();

        void AddSourcePath();
        void AddTargetPath();
    }
}