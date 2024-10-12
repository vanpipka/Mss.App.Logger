using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using Mss.App.Logger.Utils.SQLUtils;
using Mss.App.Logger.Constants.SqlConstants;

namespace Mss.App.Logger.Persistence.Repository.Context;

/// <summary>
/// Provides a context for database operations using Dapper, allowing connections to the configured SQL Server.
/// </summary>
/// <remarks>
/// The context is initialized with a connection string retrieved from the configuration, specifically from the "MssAppLogger:MSSQLConnection" section.
/// Upon instantiation, the context also checks if the specified database exists, and if not, creates it.
/// </remarks>
/// <param name="configuration">An instance of <see cref="IConfiguration"/> used to retrieve the connection string.</param>

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _connectionString = _configuration.GetSection("MssAppLogger:MSSQLConnection").Value ?? throw new ArgumentNullException("Connection string not found.");

        var task = CreateDataBaseIfNotExists.CheckUndCreate(this);
        task.Wait();
    }

    public IDbConnection CreateConnection(string dataBaseName = "")
    {
        var databaseName = SQLConstants.DataBaseName;

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