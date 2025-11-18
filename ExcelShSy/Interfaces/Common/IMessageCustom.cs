namespace ExcelShSy.Core.Interfaces.Common
{
    public interface IMessageCustom<TOut, TIn>
    {
        TOut GetMessageBoxCustom(TIn CustomParams);
    }
}
