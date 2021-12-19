using System.Collections.Generic;
using ResultOf;

namespace TagCloud.TextProcessing
{
    public interface ITextProcessor
    {
        Result<Dictionary<string, int>> GetWordsWithFrequency(ITextProcessingOptions options, string filePath);
    }
}