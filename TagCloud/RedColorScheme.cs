using System.Drawing;
using TagCloud.Interfaces;
using TagCloud.IntermediateClasses;
using TagCloud.Result;

namespace TagCloud
{
    public class RedColorScheme : IColorScheme
    {
        public const int MaxChannelValue = 255;
        public const int MaxChannelDelta = 200;

        public Result<Color> Process(PositionedElement element)
        {
            return Color.FromArgb(MaxChannelValue - MaxChannelDelta / element.Frequency, Color.Red);
        }
    }
}