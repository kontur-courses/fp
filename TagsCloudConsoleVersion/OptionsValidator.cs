using System;
using System.Drawing;
using System.IO;
using ResultOf;
using TagsCloudGenerator.Infrastructure;

namespace TagsCloudConsoleVersion
{
    public class OptionsValidator
    {
        public Result<Options> ValidateOptions(Options parsedOptions)
        {
            return ValidateInputFilename(parsedOptions)
                .Then(options => ValidateFontFamily(options))
                .Then(options => ValidateMinFontSize(options))
                .Then(options => ValidateWidth(options))
                .Then(options => ValidateHeight(options))
                .Then(options => ValidateImageExtension(options))
                .RefineError("Some options is defined with a bad format");
        }

        private Result<Options> ValidateFontFamily(Result<Options> options)
        {
            var fontFamily = options.GetValueOrThrow().FontFamily;
            var font = new Font(fontFamily, 14);
            return ValidateOption(
                options,
                fontFamily,
                f => string.Equals(f, font.Name),
                $"Font named {fontFamily} not found");
        }

        private Result<Options> ValidateHeight(Result<Options> options)
        {
            return ValidateIntPropertyPositivity(options, options.GetValueOrThrow().Height, "Height");
        }

        private Result<Options> ValidateImageExtension(Result<Options> options)
        {
            var imageExtension = options.GetValueOrThrow().ImageExtension;
            return ValidateOption(
                options,
                imageExtension,
                f => ImageFormatUtils.ImageFormats.ContainsKey(f),
                $"Bad image extension {imageExtension}");
        }

        private Result<Options> ValidateInputFilename(Result<Options> options)
        {
            var inputFilename = options.GetValueOrThrow().InputFilename;
            return ValidateOption(
                options,
                inputFilename,
                File.Exists,
                $"Input file {inputFilename} does not exists");
        }

        private Result<Options> ValidateIntPropertyPositivity(Result<Options> options, int value, string propertyName)
        {
            return ValidateOption(
                options,
                value,
                n => n > 0,
                $"{propertyName} must be positive");
        }

        private Result<Options> ValidateMinFontSize(Result<Options> options)
        {
            return ValidateIntPropertyPositivity(options,
                options.GetValueOrThrow().MinFontSize, "Min font size");
        }

        private Result<Options> ValidateOption<T>(Result<Options> options, T checkedOption,
            Func<T, bool> predicate, string errorMessage)
        {
            return predicate(checkedOption)
                ? options
                : Result.Fail<Options>(errorMessage);
        }

        private Result<Options> ValidateWidth(Result<Options> options)
        {
            return ValidateIntPropertyPositivity(options, options.GetValueOrThrow().Width, "Width");
        }
    }
}