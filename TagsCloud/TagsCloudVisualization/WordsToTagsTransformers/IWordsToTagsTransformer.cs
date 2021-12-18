using System.Collections.Generic;
using ResultMonad;
using TagsCloud.Utils;

namespace TagsCloudVisualization.WordsToTagsTransformers
{
    public interface IWordsToTagsTransformer
    {
        Result<IEnumerable<Tag>> Transform(IEnumerable<string> words);
    }
}