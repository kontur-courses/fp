using System.Collections.Generic;

namespace TagCloud
{
    public interface IWordFilter
    {
        Result<IEnumerable<string>> Filter(IEnumerable<string> words);
    }
}