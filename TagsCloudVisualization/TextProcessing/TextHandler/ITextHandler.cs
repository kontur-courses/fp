using System.Collections.Generic;
using TagsCloudVisualization.Words;

namespace TagsCloudVisualization.TextProcessing.TextHandler
{
    public interface ITextHandler
    {
        Result<IEnumerable<Word>> GetHandledWords(string text);
    }
}