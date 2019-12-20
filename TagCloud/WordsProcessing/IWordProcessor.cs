using System.Collections.Generic;
using ResultOf;
using TagCloud.Infrastructure;

namespace TagCloud.WordsProcessing
{
    public interface IWordProcessor
    {
        Result<IEnumerable<Word>> PrepareWords(IEnumerable<string> rawWords);
    }
}
