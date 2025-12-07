using System.Runtime.CompilerServices;

namespace ExcelShSy.Core.Interfaces.Common
{
    /// <summary>
    /// Describes logging capabilities for application diagnostics.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Initializes the logger and returns the path to the log file or storage target.
        /// </summary>
        /// <returns>Path or identifier of the log output destination.</returns>
        string Init();

        /// <summary>
        /// Logs a general message with default severity.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Log(string message, string? pref = null);

        /// <summary>
        /// Logs a message indicating an error condition.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="file">The file where the error occurred.</param>
        /// <param name="member">The member where the error occurred.</param>
        void LogError(string message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "");

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The information to log.</param>
        void LogInfo(string message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning text.</param>
        /// <param name="file">The file where the error occurred.</param>
        /// <param name="member">The member where the error occurred.</param>

        void LogWarning(string message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "");
    }
}