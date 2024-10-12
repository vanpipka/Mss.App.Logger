using Mss.App.Logger.Persistence.Repository.GenericRepository;
using Mss.App.Logger.Models;
using Mss.App.Logger.Constants;
using Mss.App.Logger.Enums;
using Microsoft.AspNetCore.Http;
using Mss.App.Logger.Utils.RequestsUtils;

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
    private readonly IGenericRepository<LogEntry> _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="MssAppLogger"/> class.
    /// </summary>
    public MssAppLogger(IGenericRepository<LogEntry> repository) 
    {
        _repository = repository;
    }

    /// <summary>
    /// Writes a log entry with the specified log level, message, exception, and stack trace.
    /// </summary>
    /// <param name="logLevel">The severity level of the log entry, based on the <see cref="LogLevel"/> enum.</param>
    /// <param name="message">A descriptive message explaining the event being logged.</param>
    /// <param name="exception">Details about any exception related to the log entry.</param>
    /// <param name="stackTrace">The stack trace of the exception, if applicable.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns the <see cref="Guid"/> of the created log entry.</returns>
    /// <exception cref="ArgumentException">Thrown if the provided log level is invalid.</exception>
    public async Task<Guid> WriteLogLine(int logLevel, string message, string exception, string stackTrace)
    {
        if (!Enum.IsDefined(typeof(LogLevel), logLevel))
        {
            throw new ArgumentException($"Invalid log level: {logLevel}");
        }
         
        var level = (LogLevel)Enum.ToObject(typeof(LogLevel), logLevel);

        var logEntry = new LogEntry(level, message, exception, stackTrace);
        var logReportId = await _repository.InsertAsync(logEntry);

        return logReportId;
    }

    /// <summary>
    /// Writes details of an HTTP request to the log, including the full URL and headers.
    /// </summary>
    /// <param name="request">The <see cref="HttpRequest"/> object containing information about the current HTTP request.</param>
    /// <param name="logLevel">The <see cref="int"/> object containing information about log level.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns the <see cref="Guid"/> of the created log entry.</returns>
    /// <remarks>
    /// This method captures the full URL and headers of an incoming HTTP request, logging them at the default log level (INFO).
    /// </remarks>
    public async Task<Guid> WriteRequestToLog(HttpRequest request, int logLevel = 0)
    {
        var fullUrl = RequestInfoExtractor.GetFullUrl(request);
        var headers = RequestInfoExtractor.GetHeadersByLine(request);

        var logRecordId = await WriteLogLine(logLevel, fullUrl, "", headers);
        return logRecordId;
    } 
}
