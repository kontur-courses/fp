using Results;
using TagsCloudVisualization.Tags;

namespace TagsCloudVisualization.CloudLayouters;

public interface ITagLayouter
{
    Result<IList<Tag>> GetTags();
}
