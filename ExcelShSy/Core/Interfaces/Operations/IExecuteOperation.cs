namespace ExcelShSy.Core.Interfaces.Operations
{
    public interface IExecuteOperation
    {
        string Name { get; }
        void Execute();
    }
}
