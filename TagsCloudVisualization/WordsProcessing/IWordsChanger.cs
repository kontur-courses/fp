using System.Collections.Generic;
using ResultOf;

namespace TagsCloudVisualization.WordsProcessing
{
    public interface IWordsChanger
    {
        Result<IEnumerable<string>> ChangeWords(IEnumerable<string> words);
    }
}