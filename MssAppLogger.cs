using Mss.App.Logger.Models;
using Mss.App.Logger.Enums;
using Mss.App.Logger.Managers.TagsManager;
using Mss.App.Logger.Persistence.Repository.Context;
using Mss.App.Logger.Utils.ModelsUtils;
using Mss.App.Logger.Extensions.LogEntryExtension;

namespace Mss.App.Logger;

/// <summary>
/// Provides a logging service for capturing and storing log entries, including custom messages and HTTP request details.
/// </summary>
/// <remarks>
/// The <see cref="MssAppLogger"/> class allows writing detailed log entries with various severity levels and exception details.
/// It uses a generic repository to persist <see cref="LogEntry"/> objects to a database. The class supports logging 
/// both general log messages and HTTP request information, such as headers and URLs.
/// </remarks>
public class MssAppLogger
{
    private readonly CommonLogEntryManager _commonLogEntryManager;
    /// <summary>
    /// Initializes a new instance of the <see cref="MssAppLogger"/> class.
    /// </summary>
    public MssAppLogger(DapperContext dbContext) 
    {
        _commonLogEntryManager = new CommonLogEntryManager(dbContext);
    }

    /// <summary>
    /// Asynchronously writes a log entry to the database by assembling a detailed log object.
    /// Validates the log level, constructs a new <see cref="LogEntry"/> object with the provided message, exception, and stack trace, 
    /// and associates optional tags with the log entry. Once assembled, the log entry is saved asynchronously.
    /// </summary>
    /// <param name="logLevelIndex">The severity level of the log entry, validated against the <see cref="LogLevel"/> enum.</param>
    /// <param name="message">A descriptive message for the log entry.</param>
    /// <param name="exception">Optional. Details about an exception, if applicable.</param>
    /// <param name="stackTrace">Optional. The stack trace information if an exception is provided.</param>
    /// <param name="tags">Optional. A list of tags associated with the log entry.</param>
    /// <returns>The created and saved <see cref="LogEntry"/> object.</returns>
    /// <exception cref="ArgumentException">Thrown if the log level is invalid.</exception>
    /// <remarks>This method ensures log entries are properly constructed and saved to the underlying data store.</remarks>
    public async Task<LogEntry> WriteLogLineAsync(
        int logLevelIndex, 
        string message, 
        string exception = "", 
        string stackTrace = "",
        List<string>? tags = null)
    {
        LogLevelValidatorConverter.Validate(logLevelIndex);

        var logEntry = await _commonLogEntryManager.CreateAssembledObject(logLevelIndex, message, exception, stackTrace, tags);

        await logEntry.SaveAsync(_commonLogEntryManager);

        return logEntry;
    }
}
