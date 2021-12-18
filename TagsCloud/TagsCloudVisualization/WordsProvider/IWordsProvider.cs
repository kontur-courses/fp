using System.Collections.Generic;
using ResultMonad;

namespace TagsCloudVisualization.WordsProvider
{
    public interface IWordsProvider
    {
        Result<IEnumerable<string>> GetWords();
    }
}