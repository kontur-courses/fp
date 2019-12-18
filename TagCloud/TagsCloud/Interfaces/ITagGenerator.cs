using System.Collections.Generic;
using TagsCloud.TagGenerators;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Interfaces
{
    public interface ITagGenerator
    {
        Result<IEnumerable<Tag>> GenerateTags(IEnumerable<(string word, int frequency)> wordsStatistics);
    }
}
