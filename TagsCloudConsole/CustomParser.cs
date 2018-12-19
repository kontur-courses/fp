using System;
using System.Drawing;
using CommandLine;
using ResultOf;

namespace TagsCloudConsole
{
    class CustomArgs
    {
        public readonly string WordsFileName;
        public readonly string Mode;
        public readonly Size ImageSize;
        public readonly Color BackgroundColor;
        public readonly Color TextColor;
        public readonly string ImageFileName;
        public readonly string FontName;

        public CustomArgs(
            string wordsFileName, 
            string mode,
            Size imageSize,
            Color backgroundColor,
            Color textColor,
            string imageFileName,
            string fontName)
        {
            WordsFileName = wordsFileName;
            Mode = mode;
            ImageSize = imageSize;
            BackgroundColor = backgroundColor;
            TextColor = textColor;
            ImageFileName = imageFileName;
            FontName = fontName;
        }
    }

    static class CustomArgsParser
    {
        public static Result<CustomArgs> ParseFromOptions(CmdOptions options)
        {
            return ParseImageSize(options.RawImageSize)
                .Then(imageSize => ParseKnownColor(options.BackgroundColorName)
                    .Then(backgroundColor => ParseKnownColor(options.TextColorName)
                        .Then(textColor => 
                            new CustomArgs(
                                options.WordsFileName, options.Mode,
                                imageSize, backgroundColor,
                                textColor, options.ImageFileName, options.FontName))));
        }

        private static Result<Color> ParseKnownColor(string colorName)
        {
            var color = Color.FromName(colorName);
            if (color.IsKnownColor)
                return color;
            return Result.Fail<Color>("Color is invalid");
        }

        private static Result<Size> ParseImageSize(string imageSizeString)
        {
            var displayParts = imageSizeString.Split('x');
            if (displayParts.Length != 2)
                return Result.Fail<Size>("Image size has invalid format");

            var leftParsed = int.TryParse(displayParts[0], out var height);
            var rightParsed = int.TryParse(displayParts[1], out var width);

            if (!(leftParsed && rightParsed))
                return Result.Fail<Size>("Image size has invalid format");
            return new Size(height, width);
        }
    }

    class CmdOptions
    {
        [Option("filename", Required = true)]
        public string WordsFileName { get; set; }

        [Option("mode", Required = false, Default = "lines")]
        public string Mode { get; set; }

        [Option("size", Required = false, Default = "600x600")]
        public string RawImageSize { get; set; }

        [Option("font", Required = false, Default = "Arial")]
        public string FontName { get; set; }

        [Option("backcolor", Required = false, Default = "Black")]
        public string BackgroundColorName { get; set; }

        [Option("textcolor", Required = false, Default = "White")]
        public string TextColorName { get; set; }

        [Option("output", Required = true)]
        public string ImageFileName { get; set; }
    }

    class CustomParser
    { 
        public Result<CustomArgs> Parse(string[] args)
        {
            return Parser.Default
                .ParseArguments<CmdOptions>(args)
                .ToResult()
                .Then(CustomArgsParser.ParseFromOptions);
        }
    }
}
