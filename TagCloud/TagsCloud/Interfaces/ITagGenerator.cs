using System.Collections.Generic;
using TagsCloud.ErrorHandling;
using TagsCloud.TagGenerators;

namespace TagsCloud.Interfaces
{
    public interface ITagGenerator
    {
        Result<IEnumerable<Tag>> GenerateTags(Dictionary<string, int> wordsStatistics);
    }
}