using System.Collections.Generic;

namespace TagsCloudResult.TextPreprocessors
{
    public interface IWordsPreprocessor
    {
        Result<IReadOnlyDictionary<string, int>> PreprocessWords(IEnumerable<string> words);
    }
}