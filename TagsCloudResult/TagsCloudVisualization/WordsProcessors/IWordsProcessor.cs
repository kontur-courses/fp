using TagsCloudVisualization.Common;
using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.WordsProcessors;

public interface IWordsProcessor
{
    public Result<IEnumerable<string>> Process(IEnumerable<string> words);
}
