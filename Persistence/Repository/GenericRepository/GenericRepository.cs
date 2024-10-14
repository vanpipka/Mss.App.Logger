using Dapper;
using System.Data;
using Mss.App.Logger.Persistence.Repository.Context;
using Mss.App.Logger.Models;
using System.Reflection;
using Mss.App.Logger.Constants.SqlConstants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mss.App.Logger.Persistence.Repository.GenericRepository;

internal class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
{
    private readonly DapperContext _dbContext;

    internal GenericRepository(DapperContext context)
    {
        _dbContext = context;
    }

    public virtual async Task<Guid> InsertAsync(T entity)
    { 
        using var connection = _dbContext.CreateConnection(SQLConstants.DataBaseName);

        var entityType = entity.GetType();
        var tableName = entityType.Name;
        var entityProperties = entityType.GetProperties();
          
        var columns = new List<string>();
        var parametersColumns = new List<string>();

        foreach (PropertyInfo property in entityProperties)
        {
            if (Attribute.IsDefined(property, typeof(NotMappedAttribute)))
            {
                continue;
            }

            columns.Add(property.Name);
            parametersColumns.Add($"@{property.Name}");
        }

        var query = $@"
            INSERT INTO {tableName} ({string.Join(", ", columns)})
            OUTPUT INSERTED.Id
            VALUES ({string.Join(", ", parametersColumns)});
        ";

        var recordId = await connection.QuerySingleAsync<Guid>(query, entity);
   
        return recordId;
    }
    
    public virtual async Task<T> GetbyIdRequeredAsync(Guid id)
    {
        var record = await GetbyIdAsync(id);

        if (record == null)
        {
            throw new InvalidOperationException($"The query did not return any records by id {id}.");
        }
        
        return record;
    }

    public virtual async Task<T?> GetByFieldValueAsync(string fieldName, object value)
    {
        using var connection = _dbContext.CreateConnection(SQLConstants.DataBaseName);

        // Build query dynamically
        var query = $"SELECT * FROM {typeof(T).Name} WHERE {fieldName} = @FieldValue";

        // Handle different types for 'value' parameter
        var parameters = new DynamicParameters();

        // Add the value as a parameter
        parameters.Add("FieldValue", value);

        // Execute the query and return the result
        var result = await connection.QuerySingleOrDefaultAsync<T>(query, parameters);

        return result;
    }

    private async Task<T?> GetbyIdAsync(Guid id)
    {
        using var connection = _dbContext.CreateConnection(SQLConstants.DataBaseName);

        var tableName = typeof(T).Name;
        var query = $"SELECT * FROM {tableName} WHERE Id = @Id;";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        var record = await connection.QuerySingleOrDefaultAsync<T>(query, param: parameters, commandType: CommandType.Text);

        return record;
    }
}
