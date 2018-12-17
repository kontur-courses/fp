using System.Drawing;
using ResultOf;

namespace TagsCloudVisualization.Infrastructure
{
    public class Palette
    {
        private readonly string backgroundColorName;
        private readonly string secondaryColorName;

        public Palette(string backgroundColorName, string secondaryColorName)
        {
            this.backgroundColorName = backgroundColorName;
            this.secondaryColorName = secondaryColorName;
        }

        public Result<(Color, SolidBrush)> GetColors()
        {
            return GetColorByName(backgroundColorName)
                .Then(bg => GetColorByName(secondaryColorName)
                    .Then(sc => (bg, new SolidBrush(sc))))
                .RefineError("No such color name");
        }

        private Result<Color> GetColorByName(string name)
        {
            return Result.Of(() => Color.FromKnownColor((KnownColor)System.Enum.Parse(typeof(KnownColor), name)));
        }
    }
}