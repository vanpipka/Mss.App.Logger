using Mss.App.Logger.Models;
using Mss.App.Logger.Managers.TagsManager;


namespace Mss.App.Logger.Extensions.LogEntryExtension;

internal static class LogEntryExtension
{
    internal static async Task<LogEntry> SaveAsync(this LogEntry logEntry, CommonLogEntryManager commonLogEntryManager)
    {
        if (string.IsNullOrEmpty(logEntry.Message))
        {
            throw new ArgumentException($"Message can not be an empty string");
        }
        await commonLogEntryManager.SaveAssembleObject(logEntry);
        return logEntry;
    }

}