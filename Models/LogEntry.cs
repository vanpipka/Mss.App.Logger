using Mss.App.Logger.Enums;

namespace Mss.App.Logger.Models;

/// <summary>
/// Represents a log entry in the logging system.
/// </summary>
public class LogEntry : BaseModel
{
    /// <summary>
    /// Gets or sets the timestamp of when the log entry was created.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the severity level of the log entry.
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// Gets or sets the message of the log entry, describing the event.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the exception details, if an error occurred.
    /// </summary>
    public string Exception { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the stack trace of the error, providing detailed context about the failure.
    /// </summary>
    public string StackTrace { get; set; } = string.Empty;

    /// <summary>
    /// Constructor for LogEntry
    /// </summary>
    public LogEntry(LogLevel logLevel, string message, string exception, string stackTrace) 
    { 
        Timestamp = DateTime.Now;
        LogLevel = logLevel;
        Message = message;
        Exception = exception;
        StackTrace = stackTrace;
    }
}
