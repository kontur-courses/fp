using System.Collections.Generic;
using System.Linq;
using TagCloud.Interfaces;
using TagCloud.IntermediateClasses;
using TagCloud.Result;

namespace TagCloudCreator
{
    public class Application
    {
        private readonly IFileReader fileReader;
        private readonly IImageSaver imageSaver;
        private readonly ICloudLayouter layouter;
        private readonly ISizeScheme sizeScheme;
        private readonly IStatisticsCollector statisticsCollector;
        private readonly IVisualizer visualizer;
        private readonly IWordFilter wordFilter;
        private readonly IWordProcessor wordProcessor;

        public Application(
            ICloudLayouter layouter,
            IVisualizer visualizer,
            IFileReader fileReader,
            IImageSaver imageSaver,
            IStatisticsCollector statisticsCollector,
            IWordFilter wordFilter,
            ISizeScheme sizeScheme,
            IWordProcessor wordProcessor)
        {
            this.layouter = layouter;
            this.visualizer = visualizer;
            this.fileReader = fileReader;
            this.imageSaver = imageSaver;
            this.statisticsCollector = statisticsCollector;
            this.wordFilter = wordFilter;
            this.sizeScheme = sizeScheme;
            this.wordProcessor = wordProcessor;
        }

        public Result<None> Run(string inputFile, string outputFile)
        {
            return fileReader.Read(inputFile)
                .Then(inp => inp.Select(s => s.ToLower()))
                .Then(inp => ProcessWords(inp, wordProcessor))
                .Then(inp => ExcludeWords(inp, wordFilter))
                .Then(inp => statisticsCollector.GetStatistics(inp))
                .Then(FillCloud)
                .Then(visualizer.Visualize)
                .Then(img => imageSaver.Save(img, outputFile));
        }

        private Result<IEnumerable<string>> ProcessWords(IEnumerable<string> words, IWordProcessor processor)
        {
            var result = new List<string>();
            foreach (var word in words)
            {
                processor.Process(word)
                    .Then(p => result.Add(p))
                    .OnFail(s => result.Add(word));
            }

            return result;
        }

        private Result<IEnumerable<string>> ExcludeWords(IEnumerable<string> words, IWordFilter filter)
        {
            return words.Where(w => !filter.ToExclude(w).GetValueOrThrow()).ToArray();
        }

        private IEnumerable<PositionedElement> FillCloud(
            IEnumerable<FrequentedWord> statistics)
        {
            var elements = new List<PositionedElement>();
            foreach (var word in statistics)
            {
                sizeScheme.GetSize(word)
                    .Then(size => layouter.PutNextRectangle(size))
                    .Then(rect => elements.Add(new PositionedElement(word, rect)));
            }

            return elements;
        }
    }
}