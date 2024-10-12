using Dapper;
using System.Data;
using Mss.App.Logger.Persistence.Repository.Context;
using Mss.App.Logger.Utils.SQLUtils;
using Mss.App.Logger.Models;
using System.Reflection;
using Mss.App.Logger.Constants.SqlConstants;

namespace Mss.App.Logger.Persistence.Repository.GenericRepository;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
{
    protected readonly DapperContext _dbContext;

    public GenericRepository(DapperContext context)
    {
        _dbContext = context;
    }

    public virtual async Task<Guid> InsertAsync(T entity)
    { 
        await CreateTableIfNotExists<T>.CheckUndCreate(_dbContext, entity);

        using var connection = _dbContext.CreateConnection(SQLConstants.DataBaseName);

        var entityType = entity.GetType();
        var tableName = entityType.Name;
        var entityProperties = entityType.GetProperties();
          
        var columns = new List<string>();
        var parametersColumns = new List<string>();

        foreach (PropertyInfo property in entityProperties)
        {
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

    public virtual async Task<T> GetAsync(Guid id)
    {
        using var connection = _dbContext.CreateConnection();

        var tableName = typeof(T).Name;
        var query = $"SELECT * FROM {tableName} WHERE Id = @Id;";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        var objects = await connection.QuerySingleOrDefaultAsync<T>(query, param: parameters, commandType: CommandType.Text);
        return objects;
    }
}
