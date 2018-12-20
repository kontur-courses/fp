using System.Collections.Generic;

namespace TagsCloudContainer.Layout
{
    public interface IWordLayout
    {
        IReadOnlyCollection<Tag> Tags { get; }
        void PlaceWords(Dictionary<string, int> wordWeights);
    }
}