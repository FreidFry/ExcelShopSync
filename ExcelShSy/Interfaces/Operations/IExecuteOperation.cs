namespace ExcelShSy.Core.Interfaces.Operations
{
    /// <summary>
    /// Represents an executable synchronization operation.
    /// </summary>
    public interface IExecuteOperation
    {
        /// <summary>
        /// Executes the operation asynchronously.
        /// </summary>
        Task Execute();

        /// <summary>
        /// Gets the collection of errors collected during the last execution.
        /// </summary>
        List<string> Errors { get; }
    }
}
