using TagsCloudVisualization.Common;

namespace TagsCloudVisualization.WordsProcessors;

public interface IWordsProcessor
{
    public Result<IEnumerable<string>> Process(IEnumerable<string> words);
}