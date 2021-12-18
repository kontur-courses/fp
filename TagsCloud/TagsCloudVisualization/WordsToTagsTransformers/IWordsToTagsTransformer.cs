using System.Collections.Generic;
using ResultMonad;

namespace TagsCloudVisualization.WordsToTagsTransformers
{
    public interface IWordsToTagsTransformer
    {
        Result<IEnumerable<Tag>> Transform(IEnumerable<string> words);
    }
}