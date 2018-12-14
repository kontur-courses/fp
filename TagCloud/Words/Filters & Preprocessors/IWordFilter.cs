using System.Collections.Generic;

namespace TagCloud.Words
{
    public interface IWordFilter
    {
        Result<IEnumerable<string>> Filter(IEnumerable<string> words);
    }
}