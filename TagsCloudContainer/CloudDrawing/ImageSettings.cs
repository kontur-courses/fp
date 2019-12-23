using System;
using System.Drawing;
using ResultOf;

namespace CloudDrawing
{
    public class ImageSettings
    {
        private ImageSettings(Color background, Size size)
        {
            Background = background;
            Size = size;
        }

        public Color Background { get; }
        public Size Size { get; }

        public static Result<ImageSettings> GetImageSettings(string colorBackground, int width, int height)
        {
            return Result
                .Validate( colorBackground, color => Enum.TryParse(color, out KnownColor knownColor),
                    $"Не существует такого цвета {colorBackground} заданного для фона")
                .Validate(_ => width > 0, "Ширина меньше нуля, либо равна нулю")
                .Validate(_ => height > 0, "Высота меньше нуля, либо равна нулю")
                .Then(Color.FromName)
                .Then(color => new ImageSettings(color, new Size(width, height)));
        }
    }
}