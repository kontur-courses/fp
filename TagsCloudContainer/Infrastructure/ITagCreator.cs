using TagsCloudContainer.Infrastructure.Tags;

namespace TagsCloudContainer.Infrastructure;

public interface ITagCreator
{
    Result<IEnumerable<Tag>> CreateTags(IEnumerable<string> words);
}
