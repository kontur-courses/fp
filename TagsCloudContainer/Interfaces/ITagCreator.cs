using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Infrastructure.Tags;

namespace TagsCloudContainer.Interfaces;

public interface ITagCreator
{
    Result<IEnumerable<Tag>> CreateTags(IEnumerable<string> words);
}
