namespace ExcelShSy.Core.Interfaces.Common
{
    public interface ILogger
    {
        string Init();
        void Log(string message);
        void LogError(string message);
        void LogInfo(string message);
        void LogWarning(string message);
    }
}