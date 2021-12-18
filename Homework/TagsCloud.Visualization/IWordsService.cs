using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.Utils;
using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.Visualization
{
    public interface IWordsService
    {
        Result<Word[]> GetWords(IWordsReadService wordsReadService);
    }
}