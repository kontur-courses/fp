using System.Collections.Generic;
using TagCloud.Data;

namespace TagCloud.Processor
{
    public interface IWordsProcessor
    {
        Result<IEnumerable<string>> Process(IEnumerable<string> words);
    }
}