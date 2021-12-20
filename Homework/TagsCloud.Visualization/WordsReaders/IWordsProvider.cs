using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.WordsReaders
{
    public interface IWordsProvider
    {
        Result<string> Read();
    }
}