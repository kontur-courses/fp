using System.Collections.Generic;
using TagCloudResult;

namespace TagsCloudTextProcessing.WordsIntoTokensTranslators
{
    public interface IWordsIntoTokenTranslator
    {
        Result<List<Token>> TranslateIntoTokens(IEnumerable<string> words);
    }
}