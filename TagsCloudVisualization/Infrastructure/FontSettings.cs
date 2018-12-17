using System.Drawing;
using ResultOf;

namespace TagsCloudVisualization.Infrastructure
{
    public class FontSettings
    {
        private readonly string fontFamily;
        private readonly int fontStyle;
        private readonly float maxFontSize;

        public FontSettings(string fontFamily, int fontStyle, float maxFontSize = 100f)
        {
            this.fontFamily = fontFamily;
            this.fontStyle = fontStyle;
            this.maxFontSize = maxFontSize;
        }

        public Result<Font> GetFont(float currentSize, float minWeight, float maxWeight)
        {
            var fontSize = GetFontSize(currentSize, minWeight, maxWeight);
            return Result.Of(() => new Font(new FontFamily(fontFamily), fontSize, (FontStyle)fontStyle))
                .RefineError("Unable download font settings, check font family name and font style number");
        }

        private float GetFontSize(float currentSize, float minWeight, float maxWeight)
        {
            return currentSize > minWeight
                ? (maxFontSize * (currentSize - minWeight)) / (maxWeight - minWeight) : minWeight;
        }
    }
}