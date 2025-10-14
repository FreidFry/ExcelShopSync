namespace ExcelShSy.Core.Interfaces.Operations
{
    public interface IExecuteOperation
    {
        Task Execute();
        List<string> Errors { get; }
    }
}
