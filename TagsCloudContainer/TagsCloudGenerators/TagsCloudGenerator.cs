using TagsCloudContainer.WordCounters;
using TagsCloudContainer.Visualizers;
using TagsCloudContainer.Readers;
using System.Drawing;
using TagsCloudContainer.WordPreprocessors;

namespace TagsCloudContainer.TagsCloudGenerators
{
    public class TagsCloudGenerator
    {
        private readonly IWordCounter wordCounter;
        private readonly IVisualizer visualizer;
        private readonly IReader reader;
        private readonly IWordPreprocessor wordPreprocessor;

        public TagsCloudGenerator(
            IWordCounter wordCounter,
            IVisualizer visualizer,
            IReader reader,
            IWordPreprocessor wordPreprocessor)
        {
            this.wordCounter = wordCounter;
            this.visualizer = visualizer;
            this.reader = reader;
            this.wordPreprocessor = wordPreprocessor;
        }

        public Result<Bitmap> CreateTagCloud()
        {
            return reader.ReadAllLines()
                .Then(wordPreprocessor.WordPreprocessing)
                .Then(wordCounter.CountWords)
                .Then(visualizer.VisualizeCloud);
        }
    }
}
