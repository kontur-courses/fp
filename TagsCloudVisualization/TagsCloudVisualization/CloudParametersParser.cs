using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using TagsCloudVisualization.Interfaces;
using TagsCloudVisualization.PointGenerators;

namespace TagsCloudVisualization
{
    public class CloudParametersParser : ICloudParametersParser
    {
        private readonly IEnumerable<IPointGenerator> pointGenerators;

        public CloudParametersParser(IEnumerable<IPointGenerator> pointGenerators)
        {
            this.pointGenerators = pointGenerators;
        }

        public Result<CloudParameters> Parse(Options options)
        {
            return ValidateFontIsSupported(options)
                .Then(ValidateColorIsExists)
                .Then(ValidateImageFormatIsSupported)
                .Then(ValidateImageSize)
                .Then(ValidatePointGeneratorIsSupported)
                .Then(o => new CloudParameters(
                        GetImageSize(o.ImageSize),
                        GetColor(o.Color),
                        o.FontName,
                        GetPointGenerator(o.PointGenerator),
                        GetImageFormat(o.OutFormat)));
        }

        public Result<Options> ValidatePointGeneratorIsSupported(Options options)
        {
            return Result.Validate(options, o => pointGenerators.Any(g => g.GetType().Name.ToLower() == o.PointGenerator),
                $"Invalid point generator name '{options.PointGenerator}'");
        }

        private Result<Options> ValidateFontIsSupported(Options options)
        {
            return Result.Validate(options,
                o => string.Equals(new Font(o.FontName, 10).Name, o.FontName, StringComparison.InvariantCultureIgnoreCase),
                $"Invalid font name '{options.FontName}'");
        }

        private Result<Options> ValidateColorIsExists(Options options)
        {
            return Result.Validate(options, o => Color.FromName(o.Color).IsKnownColor || o.Color == "random" || o.Color == "rainbow",
                $"Invalid color name '{options.Color}'");
        }

        private Result<Options> ValidateImageSize(Options options)
        {
            return Result.Validate(options, o => o.ImageSize.Contains("x"), $"Invalid imageSize name '{options.ImageSize}'");
        }

        private Result<Options> ValidateImageFormatIsSupported(Options options)
        {
            return Result.Validate(options, o => o.OutFormat == "tiff" || o.OutFormat == "png" || o.OutFormat == "jpeg",
                $"Invalid image format '{options.OutFormat}'");
        }

        public IPointGenerator GetPointGenerator(string name)
        {
            return pointGenerators.FirstOrDefault(pointGenerator => name == pointGenerator.GetType().Name.ToLower());
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