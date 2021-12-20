using System.Collections.Generic;
using System.Drawing;
using ResultMonad;
using TagsCloudVisualization.DrawableContainers.Builders;
using TagsCloudVisualization.ImageCreators;
using TagsCloudVisualization.WordsPreprocessors;
using TagsCloudVisualization.WordsProvider;
using TagsCloudVisualization.WordsToTagTransformers;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        private readonly IFileReadService fileReadService;
        private readonly IImageCreator imageCreator;
        private readonly IWordsPreprocessor wordsProcessor;
        private readonly IWordsToTagTransformer wordsToTagTransformer;
        private readonly IDrawableContainerBuilder drawableContainerBuilder;

        public Visualizer(IFileReadService fileReadService, IWordsPreprocessor wordsProcessor,
            IWordsToTagTransformer wordsToTagTransformer, IDrawableContainerBuilder drawableContainerBuilder, 
            IImageCreator imageCreator)
        {
            this.fileReadService = fileReadService;
            this.wordsProcessor = wordsProcessor;
            this.wordsToTagTransformer = wordsToTagTransformer;
            this.drawableContainerBuilder = drawableContainerBuilder;
            this.imageCreator = imageCreator;
        }

        public Image Visualize()
        {
            return new Result<IEnumerable<string>>()
                .Then(_ => fileReadService.GetFileContent())
                .Then(words => wordsProcessor.Preprocess(words))
                .Then(words => wordsToTagTransformer.Transform(words))
                .Then(tags => drawableContainerBuilder.AddTags(tags))
                .Then(_ => drawableContainerBuilder.Build())
                .Then(container => imageCreator.Draw(container))
                .GetValueOrThrow();
        }
    }
}