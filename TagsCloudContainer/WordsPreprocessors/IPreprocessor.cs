using System.Collections.Generic;

namespace TagsCloudContainer.WordsPreprocessors
{
    public interface IWordsPreprocessor
    {
        Result<IEnumerable<string>> ProcessWords(IEnumerable<string> words);
    }
}