using System.Collections.Generic;
using TagCloudResult;

namespace TagsCloudTextProcessing.Formatters
{
    public interface IWordsFormatter
    {
        Result<IEnumerable<string>> Format(IEnumerable<string> wordsInput);
    }
}