using System.Drawing;
using System.Linq;
using ResultOf;

namespace TagCloud
{
    public class ImageSettings : IImageSettings
    {
        private readonly string textColor;
        private readonly string backgroundColor;
        private readonly string fontName;
        private readonly string sizeString;

        public ImageSettings(string textColor, string backgroundColor, string fontName)
        {
            this.textColor = textColor;
            this.backgroundColor = backgroundColor;
            this.fontName = fontName;
        }
        
        public ImageSettings(string textColor, string backgroundColor, string fontName, string sizeString)
        : this(textColor, backgroundColor, fontName)
        {
            this.sizeString = sizeString;
        }

        public Result<Color> GetTextColor() => GetColor(textColor);

        public Result<Color> GetBackgroundColor() => GetColor(backgroundColor);

        private static Result<Color> GetColor(string colorName)
        {
            return Result.Of(() => Color.FromName(colorName))
                .RefineError($"Error when trying to use color {colorName}");
        }

        public Result<Font> GetFont(float fontSize)
        {
            return Result.Of(() => new Font(fontName, fontSize))
                .RefineError($"Error when trying to use font {fontName}");
        }

        public Result<Size?> GetSize()
        {
            if (sizeString == null)
                return null;
            return Result.Of(() => sizeString.Split('x').Select(int.Parse).ToArray())
                .Then(a => new Size?(new Size(a[0], a[1])))
                .RefineError($"Error when trying to read the size argument");
        }
    }
}