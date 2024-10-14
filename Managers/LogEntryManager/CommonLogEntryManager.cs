using Microsoft.Data.SqlClient;
using Mss.App.Logger.Models;
using Mss.App.Logger.Persistence.Repository.Context;
using Mss.App.Logger.Persistence.Repository.GenericRepository;
using Mss.App.Logger.Utils.ModelsUtils;

namespace Mss.App.Logger.Managers.TagsManager;

internal class CommonLogEntryManager
{
    private readonly GenericRepository<LogEntry> _logEntryRepository;
    private readonly GenericRepository<LogEntryTags> _logEntryTagsRepository;
    private readonly DapperContext _dbContext;

    internal CommonLogEntryManager(DapperContext dbContext)
    {
        _logEntryRepository = new GenericRepository<LogEntry>(dbContext);
        _logEntryTagsRepository = new GenericRepository<LogEntryTags>(dbContext);
        _dbContext = dbContext;
    }

    internal async Task<LogEntry> CreateAssembledObject(
        int logLevelIndex,
        string message,
        string exception,
        string stackTrace,
        List<string>? namesOfTags = null)
    {
        var logLevel = LogLevelValidatorConverter.ValidateUndConvert(logLevelIndex);
        var tags = await new TagBatchManager(_dbContext).FindByNameOrCreate(namesOfTags);

        var logEntry = new LogEntry(logLevel, message, exception, stackTrace, tags);

        return logEntry;
    }

    internal async Task<bool> SaveAssembleObject(LogEntry logEntry)
    {
        try 
        { 
            await _logEntryRepository.InsertAsync(logEntry);

            foreach (var tag in logEntry.Tags)
            {
                await _logEntryTagsRepository.InsertAsync(new LogEntryTags(tag, logEntry));
            }           
        }
        catch (SqlException ex)
        {
            throw new InvalidOperationException($"Failed to save log entry to the database. SQL Error: {ex.Message}", ex);
        }
        catch (ArgumentNullException ex)
        {
            throw new ArgumentException("Log entry or one of its required properties was null.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An unexpected error occurred while saving the log entry: {ex.Message}", ex);
        }

        return true;
    }
}
