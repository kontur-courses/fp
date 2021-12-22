using System;
using System.Drawing;
using TagsCloudVisualization.Infrastructure;
using TagsCloudVisualization.Infrastructure.TextAnalysing;

namespace TagsCloudVisualization.Default
{
    public class RandomTagColor : ITagColorChooser
    {
        private Random random = new Random();
        public Color GetTokenColor(Token token)
        {
            return Color.FromArgb(random.Next(0,200), random.Next(0,200), random.Next(0,200));
        }
    }
}