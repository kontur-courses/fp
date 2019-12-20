using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Text.RegularExpressions;
using ResultOf;

namespace TagsCloudContainer
{
    public static class ArgumentsParser
    {
        public static Result<ImageFormat> ParseImageFormat(string strImageFormat)
        {
            var bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase;
            var imageFormat = (ImageFormat) typeof(ImageFormat)
                .GetProperty(strImageFormat, bindingAttr)
                ?.GetValue(null);
            return imageFormat ?? Result.Fail<ImageFormat>($"Unknown image format: {strImageFormat}");
        }

        public static Result<Color> ParseColor(string strColor)
        {
            var parsedColor = Color.FromName(strColor);
            return parsedColor.IsKnownColor
                ? parsedColor.AsResult()
                : Result.Fail<Color>($"Unknown color: {strColor}");
        }

        public static Result<Brush> ParseBrush(string strBrushColor)
        {
            var colorResult = ParseColor(strBrushColor);
            return colorResult.Then<Color, Brush>(color => new SolidBrush(color));
        }
        
        public static Result<Font> ParseFont(string fontName, int fontSize)
        {
            var font = new Font(fontName, fontSize);
            return string.Equals(font.Name, fontName, StringComparison.CurrentCultureIgnoreCase)
                ? font.AsResult()
                : Result.Fail<Font>($"Font with {fontName} name was not found in the system");
        }

        public static Result<Size> ParseSize(string strSize)
        {
            var sizeRegex = new Regex(@"(\d+)x(\d+)");
            if (!sizeRegex.IsMatch(strSize))
                return Result.Fail<Size>($"Failed parsing size {strSize}");
            
            var match = sizeRegex.Match(strSize);
            var width = Convert.ToInt32(match.Groups[1].Value);
            var height = Convert.ToInt32(match.Groups[2].Value);
            return new Size(width, height);
        }
    }
}