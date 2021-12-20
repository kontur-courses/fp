using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Tags;

namespace TagsCloudContainer.TagCreators;

public interface ITagCreator
{
    Result<IEnumerable<Tag>> CreateTags(IEnumerable<string> words);
}
