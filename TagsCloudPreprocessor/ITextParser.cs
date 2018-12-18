using System.Collections.Generic;
using ResultOfTask;

namespace TagsCloudPreprocessor
{
    public interface ITextParser
    {
        Result<IEnumerable<string>> GetWords(string text);
    }
}