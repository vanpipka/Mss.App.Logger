using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using Mss.App.Logger.Utils.SQLUtils;
using Mss.App.Logger.Constants.SqlConstants;

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

        var task = CreateDataBaseIfNotExists.CheckUndCreate(this);
        task.Wait();
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