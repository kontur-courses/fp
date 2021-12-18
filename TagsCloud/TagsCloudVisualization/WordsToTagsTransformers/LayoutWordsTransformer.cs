using System;
using System.Collections.Generic;
using System.Linq;
using ResultMonad;
using TagsCloud.Utils;

namespace TagsCloudVisualization.WordsToTagsTransformers
{
    public class LayoutWordsTransformer : IWordsToTagsTransformer
    {
        public Result<IEnumerable<Tag>> Transform(IEnumerable<string> words)
        {
            var counts = new Dictionary<string, int>();
            var maxCount = 1;

            foreach (var word in words)
            {
                counts[word] = counts.GetValueOrDefault(word, 0) + 1;
                maxCount = Math.Max(maxCount, counts[word]);
            }

            return counts.Select(pair => Tag.Create(pair.Key, pair.Value / (float)maxCount)).Traverse();
        }
    }
}