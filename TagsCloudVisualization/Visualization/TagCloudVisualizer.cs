using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.CloudPainters;
using TagsCloudVisualization.Layouters;

namespace TagsCloudVisualization.Visualization
{
    public class TagCloudVisualizer : ICloudVisualizer<Tuple<string, Rectangle>>
    {
        private readonly VisualisingOptions visualisingOptions;

        public TagCloudVisualizer(VisualisingOptions visualisingOptions)
        {
            this.visualisingOptions = visualisingOptions;
        }

        public Bitmap GetVisualization(IEnumerable<string> words, ILayouter layouter,
            ICloudPainter<Tuple<string, Rectangle>> cloudPainter)
        {
            var rectangles = GetRectanglesForWords(words, layouter);
            return cloudPainter.GetImage(words.Zip(rectangles, Tuple.Create), visualisingOptions);
        }
        
        private IEnumerable<Rectangle> GetRectanglesForWords(IEnumerable<string> words, ILayouter layouter)
        {
            var bitmap = new Bitmap(visualisingOptions.ImageSize.Width, visualisingOptions.ImageSize.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                foreach (var word in words)
                {
                    var rectangleSize = graphics.MeasureString(word, visualisingOptions.Font).ToSize();
                    yield return layouter.PutNextRectangle(rectangleSize);
                }
            }
        }
    }
}