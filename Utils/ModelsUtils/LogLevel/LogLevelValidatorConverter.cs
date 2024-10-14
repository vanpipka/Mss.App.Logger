using Mss.App.Logger.Enums;

namespace Mss.App.Logger.Utils.ModelsUtils;

internal static class LogLevelValidatorConverter
{
    internal static LogLevel ValidateUndConvert(int logLevelIndex)
    {
        Validate(logLevelIndex);
        return Convert(logLevelIndex);
    }

    internal static void Validate(int logLevelIndex)
    {
        if (!Enum.IsDefined(typeof(LogLevel), logLevelIndex))
        {
            throw new ArgumentException($"Invalid log level: {logLevelIndex}");
        }
    }
    internal static LogLevel Convert(int logLevelIndex)
    {
        var level = (LogLevel)Enum.ToObject(typeof(LogLevel), logLevelIndex);
        return level;
    }
}
