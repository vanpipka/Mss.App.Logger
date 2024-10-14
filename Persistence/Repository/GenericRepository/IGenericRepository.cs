using Mss.App.Logger.Models;

namespace Mss.App.Logger.Persistence.Repository.GenericRepository;

internal interface IGenericRepository<T> where T : BaseModel
{
    Task<Guid> InsertAsync(T entity);
    Task<T> GetbyIdRequeredAsync(Guid id);
}

