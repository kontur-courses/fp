using System.Collections.Generic;

namespace TagsCloud
{
    public interface IBoringWordsCollection
    {
        Result<IEnumerable<string>> DeleteBoringWords();
    }
}