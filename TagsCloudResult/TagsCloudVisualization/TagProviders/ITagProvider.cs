using TagsCloudVisualization.Common;
using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.WordsAnalyzers;

public interface ITagProvider
{
    public Result<IEnumerable<Tag>> GetTags();
}
