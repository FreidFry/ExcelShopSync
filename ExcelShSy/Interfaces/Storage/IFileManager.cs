namespace ExcelShSy.Core.Interfaces.Storage
{
    /// <summary>
    /// Coordinates the selection and lifecycle of file paths used during synchronization.
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Gets or sets the collection of target file paths that will be updated.
        /// </summary>
        List<string> TargetPaths { get; set; }

        /// <summary>
        /// Gets or sets the collection of source file paths that provide data.
        /// </summary>
        List<string> SourcePaths { get; set; }
        
        /// <summary>
        /// Initializes both source and target files, preparing them for processing.
        /// </summary>
        void InitializeAllFiles();

        /// <summary>
        /// Clears state and cached file paths after synchronization completes.
        /// </summary>
        void ClearAfterComplete();

        /// <summary>
        /// Adds a new source file path to the manager.
        /// </summary>
        void AddSourcePath();

        /// <summary>
        /// Adds a new target file path to the manager.
        /// </summary>
        void AddTargetPath();
    }
}