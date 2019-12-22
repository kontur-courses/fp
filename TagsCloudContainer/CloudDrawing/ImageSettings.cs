using System;
using System.Drawing;
using ResultOf;

namespace CloudDrawing
{
    public class ImageSettings
    {
        public ImageSettings(Color background, Size size)
        {
            Background = background;
            Size = size;
        }

        public static Result<ImageSettings> GetImageSettings(string colorBackground, int width, int height)
        {
            return Result.Validate(colorBackground, color => Enum.TryParse(color, out KnownColor knownColor),
                    $"Не существует такого цвета {colorBackground} заданного для фона")
                .Then(Color.FromName)
                .Then(color => new ImageSettings(color, new Size(width, height)));
        }

        public Color Background { get; }
        public Size Size { get; }
    }
}