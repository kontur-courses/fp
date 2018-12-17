using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Functional;
using TagCloudVisualization;

namespace TagCloudCreation
{
    public sealed class TagCloudCreator
    {
        private readonly ITagCloudStatsGenerator generator;
        private readonly TagCloudImageCreator imageCreator;
        private readonly IEnumerable<IWordPreparer> preparers;

        public TagCloudCreator(
            IEnumerable<IWordPreparer> preparers,
            ITagCloudStatsGenerator generator,
            TagCloudImageCreator imageCreator)
        {
            this.preparers = preparers;
            this.generator = generator;
            this.imageCreator = imageCreator;
        }

        public Result<Bitmap> CreateImage(IEnumerable<string> words, TagCloudCreationOptions options)
        {
            return PrepareWords(words, options)
                   .Then(ws => generator.GenerateStats(ws))
                   .Then(PrepareStats)
                   .Then(wis => imageCreator.CreateTagCloudImage(wis, options.ImageOptions));
        }

        private Result<IEnumerable<WordInfo>> PrepareStats(IEnumerable<WordInfo> stats)
        {
            var maxCount = stats.Max(w => w.Count);
            var minCount = stats.Min(w => w.Count);

            return Result.Of(() => stats.OrderByDescending(wi => wi.Count)
                                        .Select(wordInfo =>
                                                    wordInfo.With(GetScale(wordInfo.Count, maxCount, minCount))));
        }

        private Result<IEnumerable<string>> PrepareWords(IEnumerable<string> words, TagCloudCreationOptions options)
        {
            foreach (var preparer in preparers)
            {
                var result = preparer.PrepareWords(words, options);
                if (!result.IsSuccess)
                    return result;
                words = result.Value;
            }

            return words.AsResult();
        }

        /// <summary>
        ///     Gets relative scale for given count of words in tag cloud
        /// </summary>
        private float GetScale(int count, int maxCount, int minCount)
        {
            if (maxCount == minCount || count == minCount)
                return 1;

            return (float) Math.Ceiling((count - minCount) * imageCreator.MaxFontSize / (maxCount - minCount));
        }
    }
}
