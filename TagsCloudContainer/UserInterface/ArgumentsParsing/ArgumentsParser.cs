using ResultOf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using TagsCloudContainer.Core;

namespace TagsCloudContainer.UserInterface.ArgumentsParsing
{
    public class ArgumentsParser : IArgumentsParser<UserInterfaceArguments>
    {
        public Result<Parameters> ParseArgumentsToParameters(UserInterfaceArguments arguments)
        {
            return Result.Ok(new Parameters().WithInputFilePath(arguments.InputFilePath)
                    .WithOutputFilePath(arguments.OutputFilePath))
                .Then(parameters => parameters.WithImageSize(new Size(arguments.Width, arguments.Height)))
                .Then(parameters => parameters.WithFont(ParseFont(arguments.Font)),
                    $"Can't parse font {arguments.Font}")
                .Then(parameters => parameters.WithColors(ParseColors(arguments.Colors)),
                    $"Can't parse colors {string.Join(", ", arguments.Colors)}")
                .Then(parameters => parameters.WithImageFormat(ParseImageFormat(arguments.ImageFormat)));
        }

        private ImageFormat ParseImageFormat(string formatName)
        {
            var imageFormat = (ImageFormat)typeof(ImageFormat)
                .GetProperty(formatName, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)
                ?.GetValue(null);

            if (imageFormat == null)
            {
                throw new ArgumentException($"Can't parse image format {formatName}");
            }

            return imageFormat;
        }

        private List<Color> ParseColors(IEnumerable<string> colorsNames)
        {
            var colorConverter = new ColorConverter();
            return colorsNames.Select(name => colorConverter.ConvertFromString(name)).Cast<Color>().ToList();
        }

        private Font ParseFont(string fontName)
        {
            var fontConverter = new FontConverter();
            return fontConverter.ConvertFromString(fontName) as Font;
        }
    }
}