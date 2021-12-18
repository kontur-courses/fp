using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.Visualization
{
    public interface IWordsService
    {
        Word[] GetWords(IWordsReadService wordsReadService);
    }
}