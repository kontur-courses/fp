using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.WordProcessing
{
    public interface IWordProcessor
    {
        Result<IEnumerable<string>> ProcessWords(IEnumerable<string> words);
    }
}