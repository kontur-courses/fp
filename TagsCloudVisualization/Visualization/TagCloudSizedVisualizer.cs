using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.CloudPainters;
using TagsCloudVisualization.ErrorHandling;
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

        public Result<Bitmap> GetVisualization(IEnumerable<string> words, ILayouter layouter,
            ICloudPainter<Tuple<SizedWord, Rectangle>> cloudPainter)
        {
            var sizedWords = Result.Of(() => new FrequencyWordSizer().GetSizedWords(words,
                (int) visualisingOptions.Font.Size,
                (int) visualisingOptions.Font.Size / 2, (int) visualisingOptions.Font.Size * 5));
            if (!sizedWords.IsSuccess)
                return Result.Fail<Bitmap>(sizedWords.Error);
            
            return Result.Of(() => GetRectanglesForWords(sizedWords.Value.Value, layouter))
                .Then(rectangles =>
                    cloudPainter.GetImage(sizedWords.Value.Value.Zip(rectangles.Value, Tuple.Create),
                        visualisingOptions));
        }

        private Result<IEnumerable<Rectangle>> GetRectanglesForWords(IEnumerable<SizedWord> words, ILayouter layouter)
        {
            var rectangles = new List<Rectangle>();
            var bitmap = new Bitmap(visualisingOptions.ImageSize.Width, visualisingOptions.ImageSize.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                foreach (var word in words)
                {
                    var font = new Font(visualisingOptions.Font.FontFamily, word.Size);
                    var rectangleSize = graphics.MeasureString(word.Value, font).ToSize();
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