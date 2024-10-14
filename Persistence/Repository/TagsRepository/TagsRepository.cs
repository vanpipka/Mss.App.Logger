using Mss.App.Logger.Persistence.Repository.Context;
using Mss.App.Logger.Models;
using Mss.App.Logger.Persistence.Repository.GenericRepository;

namespace Mss.App.Logger.Persistence.Repository.TagsRepository;

internal class TagsRepository : GenericRepository<Tags>, ITagsRepository
{
    private readonly DapperContext _dbContext;

    internal TagsRepository(DapperContext context) : base(context) 
    {
        _dbContext = context;
    }

    public async Task<Tags> GetOrCreateByName(string name)
    { 
        var tag = await base.GetByFieldValueAsync("Name", name);
        
        if (tag == null)
        {
            tag = new Tags(name);
            await base.InsertAsync(tag);
        }

        return tag;
    }
}
