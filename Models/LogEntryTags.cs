namespace Mss.App.Logger.Models;

internal class LogEntryTags : BaseModel
{
    public Guid TagId { get; set; }
    public Guid LogEntryId { get; set; }

    internal LogEntryTags() { }

    internal LogEntryTags(Tags tag, LogEntry logEntry)
    {
        TagId = tag.Id;
        LogEntryId = logEntry.Id;
    }
}

