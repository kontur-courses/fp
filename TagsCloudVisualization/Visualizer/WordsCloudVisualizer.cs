using System;
using System.Collections.Generic;
using System.Drawing;
using ResultOf;
using TagsCloudVisualization.Infrastructure;
using TagsCloudVisualization.Layouter;

namespace TagsCloudVisualization.Visualizer
{
    public class WordsCloudVisualizer : IVisualizer<IWordsCloudBuilder>
    {
        private readonly Palette palette;
        private readonly IWordsCloudBuilder wordsCloudBuilder;
        private readonly Size pictureSize;
        
        public WordsCloudVisualizer(IWordsCloudBuilder wordsCloudBuilder, Palette palette, Size pictureSize)
        {
            this.palette = palette;
            this.pictureSize = pictureSize;
            this.wordsCloudBuilder = wordsCloudBuilder;
        }
        public Result<Bitmap> Draw()
        {
            return palette.GetColors()
                .Then(colors => wordsCloudBuilder.Build()
                    .Then(words => GetBitmap()
                        .Then(bmp => DrawCloud(bmp, words, colors.Item1, colors.Item2))));
        }

        private Result<Bitmap> GetBitmap()
        {
            var cloudSize = wordsCloudBuilder.Radius * 2;
            if (cloudSize > Math.Min(pictureSize.Height, pictureSize.Width))
                return Result.Fail<Bitmap>($"Cloud can't fit in given size. Size at least must be {cloudSize}x{cloudSize}");
            return Result.Of(() => new Bitmap(pictureSize.Width, pictureSize.Height));
        }
        
        private Result<Bitmap> DrawCloud(Bitmap bmp, IEnumerable<Word> words, Color backgroundColor, Brush wordsColor)
        {
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(backgroundColor);
                foreach (var word in words)
                    g.DrawString(word.Text, word.Font, wordsColor, word.Rectangle.ShiftRectangleToBitMapCenter(bmp));
            }
            return bmp;
        }
    }
}
