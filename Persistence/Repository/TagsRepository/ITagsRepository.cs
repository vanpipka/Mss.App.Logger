using Mss.App.Logger.Models;
using Mss.App.Logger.Persistence.Repository.GenericRepository;

namespace Mss.App.Logger.Persistence.Repository.TagsRepository;

internal interface ITagsRepository : IGenericRepository<Tags>
{
    Task<Tags> GetOrCreateByName(string name);
}

