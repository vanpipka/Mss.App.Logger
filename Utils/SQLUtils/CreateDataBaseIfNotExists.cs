using Dapper;
using Microsoft.Data.SqlClient;
using Mss.App.Logger.Constants.SqlConstants;
using Mss.App.Logger.Persistence.Repository.Context;
using System.Data;

namespace Mss.App.Logger.Utils.SQLUtils;

internal static class CreateDataBaseIfNotExists
{
    internal async static Task CheckUndCreate(DapperContext context)
    {
        var databaseName = SQLConstants.DataBaseName;

        using var connection = context.CreateConnection();

        var command = new CommandDefinition($@"
            DECLARE @dbCreated BIT = 0;
            IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{databaseName}')
            BEGIN
                CREATE DATABASE [{databaseName}];
            SET @dbCreated = 1;
            END
            SELECT @dbCreated AS DataBaseWasCreated;
        ");

        var dataBaseWasCreated = await connection.ExecuteScalarAsync<int>(command);

        if (dataBaseWasCreated == 1)
        {
            Console.WriteLine($"DB {databaseName} was created");
        }          
    }
}
