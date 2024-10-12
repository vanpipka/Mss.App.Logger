using Mss.App.Logger.Models;

namespace Mss.App.Logger.Persistence.Repository.GenericRepository;

/// <summary>
/// Defines a generic repository interface for performing basic CRUD operations on models that inherit from <see cref="BaseModel"/>.
/// </summary>
public interface IGenericRepository<T> where T : BaseModel
{
    /// <summary>
    /// Asynchronously inserts an entity into the database and returns its unique identifier.
    /// </summary>
    Task<Guid> InsertAsync(T entity);

    /// <summary>
    /// Asynchronously retrieves an entity from the database by its unique identifier.
    /// </summary>
    Task<T> GetAsync(Guid id);
}

