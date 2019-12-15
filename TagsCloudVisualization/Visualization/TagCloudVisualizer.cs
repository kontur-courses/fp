using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.CloudPainters;
using TagsCloudVisualization.ErrorHandling;
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

        public Result<Bitmap> GetVisualization(IEnumerable<string> words, ILayouter layouter,
            ICloudPainter<Tuple<string, Rectangle>> cloudPainter)
        {
            return Result.Of(() => GetRectanglesForWords(words, layouter))
                .Then(rectangles =>
                    cloudPainter.GetImage(words.Zip(rectangles.Value, Tuple.Create), visualisingOptions));
        }

        private Result<IEnumerable<Rectangle>> GetRectanglesForWords(IEnumerable<string> words, ILayouter layouter)
        {
            var rectangles = new List<Rectangle>();
            var bitmap = new Bitmap(visualisingOptions.ImageSize.Width, visualisingOptions.ImageSize.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                foreach (var word in words)
                {
                    var rectangleSize = graphics.MeasureString(word, visualisingOptions.Font).ToSize();
                    var nextRectangleResult = layouter.PutNextRectangle(rectangleSize);
                    if (nextRectangleResult.IsSuccess)
                        rectangles.Add(nextRectangleResult.Value);
                    else
                        return Result.Fail<IEnumerable<Rectangle>>("Failed to create word layout");
                }
            }

            return rectangles.AsEnumerable().AsResult();
        }
    }
}