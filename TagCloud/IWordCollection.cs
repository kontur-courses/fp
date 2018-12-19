using System.Collections.Generic;

namespace TagsCloud
{
    public interface IWordCollection
    {
        Result<IEnumerable<string>> GetWords();
    }
}