using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.CloudPainters;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.WordSizing;

namespace TagsCloudVisualization.Visualization
{
    public class TagCloudSizedVisualizer : ICloudVisualizer<Tuple<SizedWord, Rectangle>>
    {
        private readonly VisualisingOptions visualisingOptions;

        public TagCloudSizedVisualizer(VisualisingOptions visualisingOptions)
        {
            this.visualisingOptions = visualisingOptions;
        }

        public Bitmap GetVisualization(IEnumerable<string> words, ILayouter layouter,
            ICloudPainter<Tuple<SizedWord, Rectangle>> cloudPainter)
        {
            var sizedWords = new FrequencyWordSizer().GetSizedWords(words, (int) visualisingOptions.Font.Size,
                (int) visualisingOptions.Font.Size / 2, (int) visualisingOptions.Font.Size * 5);
            var rectangles = GetRectanglesForWords(sizedWords, layouter);
            return cloudPainter.GetImage(sizedWords.Zip(rectangles, Tuple.Create), visualisingOptions);
        }

        private IEnumerable<Rectangle> GetRectanglesForWords(IEnumerable<SizedWord> words, ILayouter layouter)
        {
            var bitmap = new Bitmap(visualisingOptions.ImageSize.Width, visualisingOptions.ImageSize.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                foreach (var word in words)
                {
                    var font = new Font(visualisingOptions.Font.FontFamily, word.Size);
                    var rectangleSize = graphics.MeasureString(word.Value, font).ToSize();
                    yield return layouter.PutNextRectangle(rectangleSize);
                }
            }
        }
    }
}