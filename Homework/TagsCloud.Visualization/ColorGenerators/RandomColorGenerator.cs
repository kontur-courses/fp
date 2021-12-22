using System;
using System.Drawing;

namespace TagsCloud.Visualization.ColorGenerators
{
    public class RandomColorGenerator : IColorGenerator
    {
        private readonly Random random = new();

        public Color Generate() =>
            Color.FromArgb(
                random.Next(175, 256),
                random.Next(256),
                random.Next(256),
                random.Next(256));
    }
}