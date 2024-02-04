using TagsCloudVisualization.Common;

namespace TagsCloudVisualization.WordsAnalyzers;

public interface ITagProvider
{
    public Result<IEnumerable<Tag>> GetTags();
}