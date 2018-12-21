using System.Drawing;
using TagCloud.Interfaces;
using TagCloud.IntermediateClasses;

namespace TagCloud
{
    public class RedColorScheme : IColorScheme
    {
        public const int MaxChannelValue = 255;
        public const int MaxChannelDelta = 200;

        public Result<Color> Process(PositionedElement element)
        {
            return Result.Of(() => Color.FromArgb(MaxChannelValue - MaxChannelDelta / element.Frequency, Color.Red));
        }
    }
}