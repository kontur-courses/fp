using System.Collections.Generic;
using System.Linq;
using ResultMonad;
using TagsCloudDrawer;
using TagsCloudVisualization.Drawable.Displayer;
using TagsCloudVisualization.Drawable.Tags.Factory;
using TagsCloudVisualization.WordsPreprocessor;
using TagsCloudVisualization.WordsProvider;
using TagsCloudVisualization.WordsToTagsTransformers;

namespace TagsCloudVisualization
{
    public class TagsCloudVisualizer
    {
        private readonly IWordsProvider _wordsProvider;
        private readonly IWordsPreprocessor _preprocessor;
        private readonly ITagDrawableFactory _tagDrawableFactory;
        private readonly IWordsToTagsTransformer _transformer;
        private readonly IDrawableDisplayer _displayer;

        public TagsCloudVisualizer(
            IWordsProvider wordsProvider,
            IWordsPreprocessor preprocessor,
            ITagDrawableFactory tagDrawableFactory,
            IWordsToTagsTransformer transformer,
            IDrawableDisplayer displayer)
        {
            _wordsProvider = wordsProvider;
            _preprocessor = preprocessor;
            _tagDrawableFactory = tagDrawableFactory;
            _transformer = transformer;
            _displayer = displayer;
        }

        public Result<None> Visualize(int limit = int.MaxValue)
        {
            return limit.AsResult()
                .Validate(v => v > 0, $"Expected {nameof(limit)} to be positive, but actual {limit}")
                .ToNone()
                .Then(_wordsProvider.GetWords)
                .Then(_preprocessor.Process)
                .Then(_transformer.Transform)
                .Then(tags => PrepareToDrawable(tags, limit))
                .Then(_displayer.Display);
        }

        private IEnumerable<IDrawable> PrepareToDrawable(IEnumerable<Tag> tags, int limit)
        {
            return tags
                .OrderByDescending(tag => tag.Weight)
                .Select(_tagDrawableFactory.Create)
                .Take(limit);
        }
    }
}