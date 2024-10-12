using Mss.App.Logger.Models;

namespace Mss.App.Logger.Persistence.Repository.GenericRepository;

public interface IGenericRepository<T> where T : BaseModel
{
    Task<Guid> InsertAsync(T entity);
    Task<T> GetAsync(Guid id);
}

