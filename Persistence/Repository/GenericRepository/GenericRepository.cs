using Dapper;
using System.Data;
using Mss.App.Logger.Persistence.Repository.Context;
using Mss.App.Logger.Utils.SQLUtils;
using Mss.App.Logger.Models;
using System.Reflection;
using Mss.App.Logger.Constants.SqlConstants;

namespace Mss.App.Logger.Persistence.Repository.GenericRepository;

/// <summary>
/// Defines a generic repository for performing basic CRUD operations on models that inherit from <see cref="BaseModel"/>.
/// </summary>
public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
{
    private readonly DapperContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepository"/> class.
    /// </summary>
    public GenericRepository(DapperContext context)
    {
        _dbContext = context;
    }

    /// <summary>
    /// Asynchronously inserts an entity into the database and returns its unique identifier.
    /// </summary>
    /// <param name="entity">The entity to insert, which must inherit from <see cref="BaseModel"/>.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns a <see cref="Guid"/> representing the unique identifier of the inserted entity.</returns>
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

    /// <summary>
    /// Asynchronously retrieves an entity from the database by its unique identifier.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of the entity to retrieve.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns the entity of type <typeparamref name="T"/> if found, or <c>null</c> if not.</returns>
    public virtual async Task<T> GetAsync(Guid id)
    {
        using var connection = _dbContext.CreateConnection();

        var tableName = typeof(T).Name;
        var query = $"SELECT * FROM {tableName} WHERE Id = @Id;";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        var record = await connection.QuerySingleOrDefaultAsync<T>(query, param: parameters, commandType: CommandType.Text);

        if (record == null)
        {
            throw new InvalidOperationException($"The query did not return any records by id {id}.");
        }
        return record;
    }
}
