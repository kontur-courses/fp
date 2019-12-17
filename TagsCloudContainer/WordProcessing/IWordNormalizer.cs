using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.WordProcessing
{
    public interface IWordNormalizer
    {
        Result<IEnumerable<string>> NormalizeWords(IEnumerable<string> words);
    }
}