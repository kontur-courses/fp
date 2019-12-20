using TagsCloudContainer.Parsing;
using TagsCloudContainer.RectangleTranslation;
using TagsCloudContainer.ResultInfrastructure;
using TagsCloudContainer.Visualization.Interfaces;
using TagsCloudContainer.Word_Counting;

namespace TagsCloudContainer
{
    public class CloudLayouter : ICloudLayouter
    {
        private readonly IFileParser parser;
        private readonly IWordCounter wordCounter;
        private readonly ISizeTranslator translator;
        private readonly IVisualizer visualizer;
        private readonly IWordLayouter layouter;


        public CloudLayouter(IFileParser parser, IWordCounter wordCounter, ISizeTranslator translator,
            IVisualizer visualizer, IWordLayouter layouter)
        {
            this.parser = parser;
            this.wordCounter = wordCounter;
            this.translator = translator;
            this.visualizer = visualizer;
            this.layouter = layouter;
        }

        public Result<None> Layout(string inputPath, string outputPath)
        {
            return parser.ParseFile(inputPath)
                .Then(parsed => wordCounter.CountWords(parsed))
                .Then(countDict => translator.TranslateWordsToSizedWords(countDict))
                .Then(translated => layouter.LayoutWords(translated))
                .Then(rectangles => visualizer.Visualize(rectangles, outputPath))
                .RefineError("Cannot layout:");
        }
    }
}