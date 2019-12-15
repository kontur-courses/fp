using System;
using System.Drawing;
using System.Text;
using TagsCloudVisualization.CloudPainters;
using TagsCloudVisualization.ErrorHandling;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.TextPreprocessing;
using TagsCloudVisualization.TextReaders;
using TagsCloudVisualization.Visualization;
using TagsCloudVisualization.WordSizing;

namespace TagsCloudVisualization
{
    public class CloudCreator
    {
        private readonly ITextReader textReader;
        private readonly WordsExtractor wordsExtractor;
        private readonly WordPreprocessor wordPreprocessor;
        private readonly ICloudPainter<Tuple<SizedWord, Rectangle>> cloudPainter;
        private readonly TagCloudSizedVisualizer tagCloudVisualizer;
        private readonly ILayouter layouter;

        public CloudCreator(ITextReader textReader, WordsExtractor wordsExtractor, WordPreprocessor wordPreprocessor,
            ICloudPainter<Tuple<SizedWord, Rectangle>> cloudPainter, TagCloudSizedVisualizer tagCloudVisualizer,
            ILayouter layouter)
        {
            this.textReader = textReader;
            this.wordsExtractor = wordsExtractor;
            this.wordPreprocessor = wordPreprocessor;
            this.cloudPainter = cloudPainter;
            this.tagCloudVisualizer = tagCloudVisualizer;
            this.layouter = layouter;
        }

        public Result<Bitmap> GetCloud(string textPath)
        {
            return Result.Of(() => textReader.ReadText(textPath, Encoding.UTF8))
                .Then(text => wordsExtractor.GetWords(text.Value))
                .Then(words => wordPreprocessor.GetPreprocessedWords(words))
                .Then(words => tagCloudVisualizer.GetVisualization(words, layouter, cloudPainter));
        }
    }
}