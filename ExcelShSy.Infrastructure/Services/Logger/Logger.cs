using ExcelShSy.Core.Interfaces.Common;
using System.Text;

namespace ExcelShSy.Infrastructure.Services.Logger
{
    public class Logger : ILogger
    {
        private readonly string _date = DateTime.Now.ToString("y-MM-dd HH-mm-ss");
        private readonly string _logFilePath;

        public Logger()
        {
            _logFilePath = Init();
        }

        public string Init()
        {
            var logDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");

            if (!Path.Exists(logDirectory)) Directory.CreateDirectory(logDirectory);
            var logPath = Path.Combine(logDirectory, "Log_" + _date + ".log");

            using (new FileStream(logPath, FileMode.CreateNew, FileAccess.Write))
            { }

            var logFiles = Directory.GetFiles(logDirectory, "*.log")
                                    .OrderBy(File.GetCreationTime)
                                    .ToList();

            while (logFiles.Count >= 5)
            {
                File.Delete(logFiles[0]);
                logFiles.RemoveAt(0);
            }

            return logPath;
        }

        public void Log(string message)
        {
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

            try
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Ошибка при записи в лог: " + ex.Message);
            }
        }

        public void LogError(string message) => Log("ERROR: " + message);

        public void LogInfo(string message) => Log("INFO: " + message);

        public void LogWarning(string message) => Log("WARNING: " + message);
    }
}
