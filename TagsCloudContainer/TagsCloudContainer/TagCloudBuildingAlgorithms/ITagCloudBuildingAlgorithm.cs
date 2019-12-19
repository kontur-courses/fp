using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface ITagCloudBuildingAlgorithm
    {
        Result<IEnumerable<Tag>> GetTags(Dictionary<string, int> wordsFrequencyDict);
    }
}