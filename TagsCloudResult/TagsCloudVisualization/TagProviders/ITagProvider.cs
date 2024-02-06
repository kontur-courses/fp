using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.TagProviders;

public interface ITagProvider
{
    public Result<IEnumerable<Tag>> GetTags();
}
