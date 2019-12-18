using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudContainer.Api;
using TagCloudContainer.ResultMonad;

namespace TagCloudContainer.Implementations
{
    [CliElement("wordlayouter")]
    public class WordCloudLayouter : IWordCloudLayouter
    {
        private readonly ICloudLayouter rectangleLayouter;
        private readonly IStringSizeProvider sizeProvider;

        public WordCloudLayouter(ICloudLayouter rectangleLayouter, IStringSizeProvider sizeProvider)
        {
            this.rectangleLayouter = rectangleLayouter;
            this.sizeProvider = sizeProvider;
        }

        public Result<IReadOnlyDictionary<string, Rectangle>> AddWords(
            IReadOnlyDictionary<string, int> words, List<Rectangle> container)
        {
            return Result.Ok(words)
                .Then(w => w.OrderByDescending(pair => pair.Value))
                .Then(w => w.Select(pair => CreateBoundingRectangle(pair.Key, pair.Value, container)))
                .Then(w => w.Select(r => r.GetValueOrThrow()))
                .Then(w => w.ToDictionary(p => p.word, p => p.rect))
                .Then(d => (IReadOnlyDictionary<string, Rectangle>) d);
        }

        private Result<(string word, Rectangle rect)> CreateBoundingRectangle(string word, int occurrenceCount,
            List<Rectangle> container)
        {
            return sizeProvider.GetStringSize(word, occurrenceCount)
                .Then(s => s * occurrenceCount)
                .Then(s => rectangleLayouter.PutNextRectangle(s, container))
                .Then(r => (word, r));
        }
    }
}