using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer.WordsPreprocessors
{
    public class WordsPreprocessor : IWordsPreprocessor
    {
        public Result<IEnumerable<string>> ProcessWords(IEnumerable<string> words)
        {
            return words
                .Where(word => word != string.Empty)
                .Select(word => word.ToLower())
                .AsResult();
        }
    }
}