namespace ExcelShSy.Core.Interfaces.Operations
{
    public interface IExecuteOperation
    {
        void Execute();
        List<string> Errors { get; }
    }
}
