using Mss.App.Logger.Models;
using Mss.App.Logger.Persistence.Repository.Context;
using Mss.App.Logger.Persistence.Repository.TagsRepository;

namespace Mss.App.Logger.Managers.TagsManager;

internal class TagBatchManager
{
    private readonly TagsRepository _tagsRepository;

    internal TagBatchManager(DapperContext dbContext)
    {
        _tagsRepository = new TagsRepository(dbContext);
    }

    internal async Task<List<Tags>> FindByNameOrCreate(List<string>? tagsNames)
    {
        var tags = new List<Tags>();

        if (tagsNames == null)
        {
            return tags;
        }

        foreach (var tagName in tagsNames)
        {
            var tag = await _tagsRepository.GetOrCreateByName(tagName);
            tags.Add(tag);
        }

        return tags;
    }
}
