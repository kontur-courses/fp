using System.Collections.Generic;
using ResultOf;
using TagCloud.Algorithm;
using TagCloud.App;
using TagCloud.Infrastructure;
using TagCloud.Visualization.WordPainting;

namespace TagCloud.Visualization
{
    public class TagCloudElementsPreparer : ITagCloudElementsPreparer
    {
        public int CurrentWordIndex { get; private set; }

        private readonly ITagCloudLayouter tagCloudLayouter;
        private readonly IWordPainterFabric wordPainterFabric;
        private readonly PictureConfig config;

        public TagCloudElementsPreparer(
            ITagCloudLayouter tagCloudLayouter, 
            IWordPainterFabric wordPainterFabric,
            PictureConfig config)
        {
            this.tagCloudLayouter = tagCloudLayouter;
            this.wordPainterFabric = wordPainterFabric;
            this.config = config;
        }

        public Result<IEnumerable<TagCloudElement>> PrepareTagCloudElements(IEnumerable<Word> words)
        {
            var wordPainterResult = wordPainterFabric.GetWordPainter();
            return !wordPainterResult.IsSuccess 
                ? Result.Fail<IEnumerable<TagCloudElement>>(wordPainterResult.Error) 
                : Result.Ok(PrepareTagCloudElements(words, wordPainterResult.GetValueOrThrow()));
        }

        private IEnumerable<TagCloudElement> PrepareTagCloudElements(
            IEnumerable<Word> words, IWordPainter wordPainter)
        {
            foreach (var word in words)
            {
                var rectangle = tagCloudLayouter.PutNextRectangle(word.WordRectangleSize);
                var color = wordPainter.GetWordColor(word);
                yield return new TagCloudElement(word, rectangle, color, config.FontFamily);
                CurrentWordIndex++;
            }
        }


    }
}