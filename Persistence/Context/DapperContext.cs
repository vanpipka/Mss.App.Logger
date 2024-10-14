using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using Mss.App.Logger.Utils.SQLUtils;
using Mss.App.Logger.Models;
using Mss.App.Logger.Enums;


namespace Mss.App.Logger.Persistence.Repository.Context;

/// <summary>
/// Provides a context for establishing and managing SQL Server database connections using Dapper.
/// </summary>
public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="DapperContext"/> class.
    /// </summary>
    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _connectionString = _configuration.GetSection("MssAppLogger:MSSQLConnection").Value ?? string.Empty;

        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new ArgumentNullException(nameof(configuration), "Connection string not found.");
        } 
    }

    internal async Task InitializeAsync()
    {
        await CreateDataBaseIfNotExists.CheckUndCreate(this);
        await CreateTableIfNotExists<LogEntry>.CheckUndCreate(this, new LogEntry(LogLevel.info, "Init", string.Empty, string.Empty));
        await CreateTableIfNotExists<Tags>.CheckUndCreate(this, new Tags(string.Empty));
        await CreateTableIfNotExists<LogEntryTags>.CheckUndCreate(this, new LogEntryTags());
    }

    internal IDbConnection CreateConnection(string dataBaseName = "")
    {
        var sqlConnection = new SqlConnection(_connectionString);

        if (sqlConnection.State != ConnectionState.Open)
        {
            sqlConnection.Open();
        }

        if (!string.IsNullOrEmpty(dataBaseName))
        {
            sqlConnection.ChangeDatabase(dataBaseName);
        }

        return sqlConnection;
    }
}