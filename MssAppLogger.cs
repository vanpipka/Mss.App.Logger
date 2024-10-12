using Mss.App.Logger.Persistence.Repository.GenericRepository;
using Mss.App.Logger.Models;
using Mss.App.Logger.Enums;
using Microsoft.AspNetCore.Http;
using Mss.App.Logger.Utils.RequestsUtils;
using Azure.Core;

namespace Mss.App.Logger;

public class MssAppLogger
{
    private readonly IGenericRepository<LogEntry> _repository;

    public MssAppLogger(IGenericRepository<LogEntry> repository) 
    {
        _repository = repository;
    }

    public async Task<Guid> WriteLogLine(int logLevel, string message, string exception, string stackTrace)
    {
        // TODO
        var level = (LogLevel)Enum.GetValues(typeof(LogLevel)).GetValue(logLevel);
        var logEntry = new LogEntry(level, message, exception, stackTrace);
        var logReportId = await _repository.InsertAsync(logEntry);

        return logReportId;
    }

    public async Task<Guid> WriteRequestToLog(HttpRequest request)
    {
        var defaultLogLevel = 0; // INFO
        var fullUrl = RequestInfoExtractor.GetFullUrl(request);
        var headers = RequestInfoExtractor.GetHeadersByLine(request);

        var logRecordId = await WriteLogLine(defaultLogLevel, fullUrl, "", headers);
        return logRecordId;
    } 
}
