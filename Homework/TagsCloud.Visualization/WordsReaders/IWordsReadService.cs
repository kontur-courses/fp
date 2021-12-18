using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.WordsReaders
{
    public interface IWordsReadService
    {
        Result<string> Read();
    }
}