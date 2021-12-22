using TagsCloudContainer.Common;
using TagsCloudContainer.Common.Result;
using TagsCloudContainer.Layouters;
using TagsCloudContainer.Painting;
using TagsCloudContainer.Preprocessors;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudContainer
{
    internal class Processor
    {
        private readonly IResultHandler handler;
        private readonly TagReader tagReader;
        private readonly WordsCountParser parser;
        private readonly TagsPreprocessor preprocessor;
        private readonly TagLayouter layouter;
        private readonly TagPainter painter;
        private readonly IVisualizator visualizator;
        private readonly IVisualizatorSettings settings;

        public Processor(IResultHandler handler,
            TagReader tagReader,
            WordsCountParser parser,
            TagsPreprocessor preprocessor,
            TagLayouter layouter,
            TagPainter painter,
            IVisualizator visualizator,
            IVisualizatorSettings settings)
        {
            this.handler = handler;
            this.tagReader = tagReader;
            this.parser = parser;
            this.preprocessor = preprocessor;
            this.layouter = layouter;
            this.painter = painter;
            this.visualizator = visualizator;
            this.settings = settings;
        }

        public void Process()
        {
            Result.Of(() => tagReader.Read(AppSettings.TextFilename),
                    "File not exist, or unavailable")
                .Then(text => parser.Parse(text))
                .Then(tags => preprocessor.Process(tags))
                .Then(tags => layouter.PlaceTagsInCloud
                    (tags, AppSettings.MinTagHeight, AppSettings.MaxTagSizeScale))
                .Then(cloud =>
                {
                    painter.SetPalettes(cloud.Elements);
                    return cloud;
                })
                .Then(cloud => visualizator.Visualize(settings, cloud))
                .RefineError("Can`t end visualizing tags")
                .OnFail(handler.AddHandledText);
        }
    }
}