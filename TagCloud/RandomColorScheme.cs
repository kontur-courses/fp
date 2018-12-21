using System;
using System.Drawing;
using TagCloud.Interfaces;
using TagCloud.IntermediateClasses;

namespace TagCloud
{
    public class RandomColorScheme : IColorScheme
    {
        private const int MaxChannelValue = 256;
        private readonly Random random = new Random();

        public Result<Color> Process(PositionedElement element)
        {
            return Result.Of(() => Color.FromArgb(
                random.Next(MaxChannelValue),
                random.Next(MaxChannelValue),
                random.Next(MaxChannelValue)));
        }
    }
}