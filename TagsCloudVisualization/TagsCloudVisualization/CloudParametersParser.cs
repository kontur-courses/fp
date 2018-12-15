using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class CloudParametersParser : ICloudParametersParser
    {
        private readonly IPointGeneratorDetector pointGeneratorDetector;

        public CloudParametersParser(IPointGeneratorDetector pointGeneratorDetector)
        {
            this.pointGeneratorDetector = pointGeneratorDetector;
        }

        public Result<CloudParameters> Parse(Options options)
        {
            return ValidateFontIsSupported(options.FontName)
                .Then(x => ValidateColorIsExists(options.Color))
                .Then(x => ValidateImageFormatIsSupported(options.OutFormat))
                .Then(x => ValidateImageSize(options.ImageSize))
                .Then(x => pointGeneratorDetector.ValidatePointGeneratorIsSupported(options.PointGenerator))
                .Then(x => new CloudParameters(
                        GetImageSize(options.ImageSize),
                        GetColor(options.Color),
                        options.FontName,
                        pointGeneratorDetector.GetPointGenerator(options.PointGenerator),
                        GetImageFormat(options.OutFormat))
                    .AsResult())
                .OnFail(error => MessageBox.Show(error, "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error));
        }

        private Result<string> ValidateFontIsSupported(string fontName)
        {
            return Result.Validate(fontName,
                f => string.Equals(new Font(f, 10).Name, f, StringComparison.InvariantCultureIgnoreCase),
                $"Invalid font name '{fontName}'");
        }

        private Result<string> ValidateColorIsExists(string colorName)
        {
            return Result.Validate(colorName, c => Color.FromName(c).IsKnownColor || c == "random" || c == "rainbow",
                $"Invalid color name '{colorName}'");
        }

        private Result<string> ValidateImageSize(string imageSize)
        {
            return Result.Validate(imageSize, s => s.Contains("x"), $"Invalid imageSize name '{imageSize}'");
        }

        private Result<string> ValidateImageFormatIsSupported(string imageFormat)
        {
            return Result.Validate(imageFormat, f => f == "tiff" || f == "png" || f == "jpeg",
                $"Invalid image format '{imageFormat}'");
        }

        private ImageFormat GetImageFormat(string input)
        {
            switch (input)
            {
                case "tiff":
                    return ImageFormat.Tiff;
                case "png":
                    return ImageFormat.Png;
                default:
                case "jpeg":
                    return ImageFormat.Jpeg;
            }
        }

        private static Func<float, Color> GetColor(string input)
        {
            var rnd = new Random();
            switch (input)
            {
                case "random":
                    return x => Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                case "rainbow":
                    return GetRainbowColor;
                default:
                    return x => Color.FromName(input);
            }
        }

        private Size GetImageSize(string input)
        {
            var delimiter = input.IndexOf('x');
            int.TryParse(input.Substring(0, delimiter), out var width);
            int.TryParse(input.Substring(delimiter + 1), out var height);
            return new Size(width, height);
        }

        public static Color GetRainbowColor(float progress)
        {
            var div = Math.Abs(progress % 1) * 6;
            var ascending = (int) (div % 1 * 255);
            var descending = 255 - ascending;

            switch ((int) div)
            {
                case 0:
                    return Color.FromArgb(255, 255, ascending, 0);
                case 1:
                    return Color.FromArgb(255, descending, 255, 0);
                case 2:
                    return Color.FromArgb(255, 0, 255, ascending);
                case 3:
                    return Color.FromArgb(255, 0, descending, 255);
                case 4:
                    return Color.FromArgb(255, ascending, 0, 255);
                default:
                    return Color.FromArgb(255, 255, 0, descending);
            }
        }
    }
}