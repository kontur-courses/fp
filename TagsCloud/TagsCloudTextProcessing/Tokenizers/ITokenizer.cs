using System.Collections.Generic;
using TagCloudResult;

namespace TagsCloudTextProcessing.Tokenizers
{
    public interface ITokenizer
    {
        Result<IEnumerable<string>> Tokenize(string text);
    }
}