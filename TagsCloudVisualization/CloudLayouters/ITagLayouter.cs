using Results;
using TagsCloudVisualization.Tags;

namespace TagsCloudVisualization.CloudLayouters;

public interface ITagLayouter
{
    IEnumerable<Result<Tag>> GetTags();
}
