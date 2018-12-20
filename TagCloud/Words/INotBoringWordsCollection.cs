using System.Collections.Generic;

namespace TagsCloud.Words
{
    public interface IBoringWordsCollection
    {
        Result<List<string>> DeleteBoringWords();
    }
}