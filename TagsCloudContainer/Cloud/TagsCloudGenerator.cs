using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.Core.Layouters;
using TagsCloudContainer.Data;
using TagsCloudContainer.Data.Processors;
using TagsCloudContainer.Data.Readers;
using TagsCloudContainer.Functional;
using TagsCloudContainer.Visualization;
using TagsCloudContainer.Visualization.Measurers;
using TagsCloudContainer.Visualization.Painters;

namespace TagsCloudContainer.Cloud
{
    public class TagsCloudGenerator
    {
        private readonly IRectangleLayouter layouter;
        private readonly IWordsFileReader wordReader;
        private readonly IEnumerable<IWordProcessor> processors;
        private readonly IWordMeasurer wordMeasurer;
        private readonly IPainter painter;
        private readonly TagsCloudVisualizer visualizer;

        public TagsCloudGenerator(
            IWordsFileReader wordReader,
            IEnumerable<IWordProcessor> processors,
            IWordMeasurer wordMeasurer,
            IRectangleLayouter layouter,
            IPainter painter,
            TagsCloudVisualizer visualizer)
        {
            this.layouter = layouter;
            this.wordReader = wordReader;
            this.processors = processors;
            this.wordMeasurer = wordMeasurer;
            this.painter = painter;
            this.visualizer = visualizer;
        }

        public Result<Bitmap> Create(TagsCloudSettings settings)
        {
            return wordReader.ReadAllWords(settings.WordsPath)
                .Then(HandleWords)
                .Then(WordCounter.Count)
                .Map(CreateTag)
                .Then(tags => tags.ToArray())
                .Then(painter.Colorize)
                .Then(visualizer.Visualize);
        }

        private Result<string[]> HandleWords(IEnumerable<string> words)
        {
            return processors
                .Aggregate(words.AsResult(), (current, processor) => current.Then(processor.Process))
                .Then(s => s.ToArray());
        }

        private Result<Tag> CreateTag(Word word)
        {
            return wordMeasurer.Measure(word)
                .SelectMany(tuple => layouter.PutNextRectangle(tuple.size),
                    (tuple, rectangle) => new Tag(word.Value, tuple.font, rectangle));
        }
    }
}