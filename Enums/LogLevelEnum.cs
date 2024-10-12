namespace Mss.App.Logger.Enums;

/// <summary>
/// Specifies the severity level of a log message.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Informational messages that represent normal application flow.
    /// </summary>
    info = 0,

    /// <summary>
    /// Warning messages that indicate potential problems or non-critical issues.
    /// </summary>
    warning = 1,

    /// <summary>
    /// Error messages that indicate a failure in the application or system.
    /// </summary>
    error = 2
}
