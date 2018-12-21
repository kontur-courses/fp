using System.Collections.Generic;
using ResultOf;

namespace TagCloud
{
    public interface IWordSource
    {
        Result<IEnumerable<string>> GetWords();
    }
}