using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Layout
{
    public interface IWordLayout
    {
        Result<HashSet<Tag>> PlaceWords(Dictionary<string, int> wordWeights);
    }
}