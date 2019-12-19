using System.Drawing;
using System.IO;
using System.Linq;
using TagsCloudContainer.Functional;

namespace TagsCloudContainer.Clients.CLI
{
    public static class OptionValidatorExtension
    {
        internal static Result<Options> Validate(this Options options)
        {
            return ValidateFile(options, options.WordsPath)
                .Then(o => o.ValidateFile(o.BoringWordsPath))
                .Then(o => o.ValidateFile(o.AffPath))
                .Then(o => o.ValidateFile(o.DicPath))
                .Then(o => o.ValidateColor(o.BorderColor))
                .Then(o => o.ValidateColor(o.FillColor))
                .Then(o => o.ValidateColor(o.TextColor))
                .Then(o => o.ValidateColor(o.PrimaryColor))
                .Then(o => o.ValidateColor(o.MajorityColor))
                .Then(o => o.ValidateColor(o.MinorityColor))
                .Then(o => o.ValidateFontFamily(o.Font))
                .Then(o => o.ValidateSizeFactor(o.SizeFactor))
                .Then(o => o.ValidateSize(o.Width, o.Height));
        }

        private static Result<Options> ValidateFile(this Options options, string file)
        {
            return File.Exists(file)
                ? options
                : Result.Fail<Options>($"File {file} not found");
        }

        private static Result<Options> ValidateColor(this Options options, string color)
        {
            return Result.Of(() => Color.FromName(color)).Then(_ => options);
        }

        private static Result<Options> ValidateFontFamily(this Options options, string font)
        {
            return FontFamily.Families.Select(family => family.Name).Contains(font)
                ? options
                : Result.Fail<Options>($"Unknown font family {font}");
        }

        private static Result<Options> ValidateSizeFactor(this Options options, float factor)
        {
            return factor > 0
                ? options
                : Result.Fail<Options>($"The size factor {factor} must be positive.");
        }

        private static Result<Options> ValidateSize(this Options options, int? width, int? height)
        {
            return !width.HasValue && !height.HasValue || width > 0 && height > 0
                ? options
                : Result.Fail<Options>($"Width {width} and height {height} must be non-negative");
        }
    }
}