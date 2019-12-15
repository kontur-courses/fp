using System;
using System.Drawing;
using TagCloudContainer.Api;

namespace TagCloudContainer.Implementations
{
    [CliElement("randomcolorbrush")]
    public class RandomColorBrushProvider : IWordBrushProvider
    {
        private readonly DrawingOptions options;
        private Random random;

        public RandomColorBrushProvider(DrawingOptions options)
        {
            this.options = options;
            this.random = new Random();
        }

        public Brush CreateBrushForWord(string word, int occurrenceCount)
        {
            return new SolidBrush(Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
        }
    }
}