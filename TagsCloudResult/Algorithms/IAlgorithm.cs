using System.Collections.Generic;

namespace TagsCloudResult.Algorithms
{
    public interface IAlgorithm
    {
        Result<IReadOnlyCollection<Tag>> GenerateTags(IReadOnlyDictionary<string, int> processedWords);
    }
}