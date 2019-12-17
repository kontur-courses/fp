using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.WordProcessing
{
    public interface IWordProvider
    {
        Result<IEnumerable<string>> GetWords();
    }
}