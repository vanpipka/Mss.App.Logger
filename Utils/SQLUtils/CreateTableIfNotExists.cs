using Dapper;
using Mss.App.Logger.Constants.SqlConstants;
using Mss.App.Logger.Models;
using Mss.App.Logger.Persistence.Repository.Context;
using System.Data;

namespace Mss.App.Logger.Utils.SQLUtils;

public static class CreateTableIfNotExists<T> where T : BaseModel
{
    public async static Task CheckUndCreate(DapperContext context, T model)
    {
        using var connection = context.CreateConnection(SQLConstants.DataBaseName);

        if (await TableExists(model, connection))
        {
            return;        
        }

        await CreateTable(model, connection);
    }

    private async static Task<bool> TableExists(T model, IDbConnection connection)
    {
        var command = new CommandDefinition($@"
                SELECT CASE 
                   WHEN EXISTS (SELECT * 
                                FROM INFORMATION_SCHEMA.TABLES 
                                WHERE TABLE_NAME = '{typeof(T).Name}' 
                                AND TABLE_SCHEMA = 'dbo') 
                   THEN 1
                   ELSE 0
               END AS TableExists;");

        var tableExists = await connection.QuerySingleAsync<int>(command);

        return tableExists == 1;
    }

    private async static Task<bool> CreateTable(T model, IDbConnection connection)
    {
        var command = new CommandDefinition(GenerateQueryText(model));
        var tableWasCreated = await connection.ExecuteAsync(command);

        if (tableWasCreated != -1)
        {
            Console.WriteLine($"Table {typeof(T).Name} was created");
        }

        return tableWasCreated != -1;
    }

    private static string GenerateQueryText(T model)
    {
        var tableName = typeof(T).Name; // Имя таблицы будет таким же, как и имя модели
        var properties = typeof(T).GetProperties();

        var columns = new List<string>();

        foreach (var property in properties)
        {
            var columnName = property.Name;
            var columnType = property.PropertyType.Name; // Получаем тип свойства

            // Генерируем SQL-тип на основе типа C#
            string sqlType = columnType switch
            {
                "Int32" => "INT",
                "String" => "NVARCHAR(MAX)",
                "DateTime" => "DATETIME",
                "Guid" => "UNIQUEIDENTIFIER",
                // Добавьте другие типы по мере необходимости
                _ => "NVARCHAR(MAX)"
            };

            columns.Add($"{columnName} {sqlType}");
        }

        return $"CREATE TABLE {tableName} ({string.Join(", ", columns)});";
    }
}
