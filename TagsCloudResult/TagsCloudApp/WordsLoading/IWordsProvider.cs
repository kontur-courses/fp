using System.Collections.Generic;
using TagsCloudContainer.Results;

namespace TagsCloudApp.WordsLoading
{
    public interface IWordsProvider
    {
        Result<IEnumerable<string>> GetWords();
    }
}