using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.WordsProcessors;

public class SimpleWordsProcessor : IWordsProcessor
{
    public Result<IEnumerable<string>> Process(IEnumerable<string> words)
    {
        return words.Select(x => x.ToLower()).Where(y => y.Length > 3).AsResult();
    }
}
