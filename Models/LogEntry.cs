using Mss.App.Logger.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mss.App.Logger.Models;

/// <summary>
/// Represents a log entry in the logging system.
/// </summary>
public class LogEntry : BaseModel
{
    /// <summary>
    /// Gets or sets the timestamp of when the log entry was created.
    /// </summary>
    public DateTime Timestamp { get; init; }

    /// <summary>
    /// Gets or sets the severity level of the log entry.
    /// </summary>
    public LogLevel LogLevel { get; init; }

    /// <summary>
    /// Gets or sets the message of the log entry, describing the event.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the exception details, if an error occurred.
    /// </summary>
    public string Exception { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the stack trace of the error, providing detailed context about the failure.
    /// </summary>
    public string StackTrace { get; init; } = string.Empty;

    ///<summary>
    /// Represents a tag of record.
    /// </summary>
    [NotMapped]
    public List<Tags> Tags { get; init; } = new List<Tags>();

    /// <summary>
    /// Constructor for LogEntry
    /// </summary>
    public LogEntry(LogLevel logLevel, string message, string exception, string stackTrace, List<Tags>? tags = null) 
    { 
        Timestamp = DateTime.Now;
        LogLevel = logLevel;
        Message = message;
        Exception = exception;
        StackTrace = stackTrace;
        Tags = (tags == null ? [] : tags);
    }
}
