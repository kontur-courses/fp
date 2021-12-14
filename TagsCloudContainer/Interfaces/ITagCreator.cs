using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.Interfaces;

public interface ITagCreator
{
    Result<IEnumerable<Tag>> CreateTags(IEnumerable<string> words);
}
