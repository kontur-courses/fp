using System.Collections.Generic;
using TagsCloudContainer;

namespace TagsCloudApp.WordsLoading
{
    public interface IWordsProvider
    {
        Result<IEnumerable<string>> GetWords();
    }
}