using ResultOf;
using System.Collections.Generic;

namespace TagsCloudContainer.WordProcessing
{
    public interface IWordProcessor
    {
        Result<IEnumerable<string>> ProcessWords(IEnumerable<string> words);
    }
}