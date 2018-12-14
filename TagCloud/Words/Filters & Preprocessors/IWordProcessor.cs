using System.Collections.Generic;

namespace TagCloud.Words
{
    public interface IWordProcessor
    {
        IEnumerable<string> Preprocess(IEnumerable<string> words);
    }
}