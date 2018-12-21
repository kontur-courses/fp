using System.Collections.Generic;
using TagsCloudResult;

namespace TagsCloudContainer.WordsFilter.BoringWords
{
    public interface IBoringWords
    {
        Result<HashSet<string>> GetBoringWords { get; }
    }
}
