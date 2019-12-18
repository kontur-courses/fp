using CommandLine;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using TagsCloud;
using TagsCloud.ErrorHandling;

namespace TagCloudCLI
{
    static class TagCloudSettingsExtensions
    {
        public static Result<TagCloudSettings> GetCloudSettingsFromArguments(ParserResult<Options> parserResult)
        {
            Result<TagCloudSettings> settings = null;
            parserResult
              .WithParsed(opts =>
              {
                  settings = TypesCollector.GetFormatFromPathSaveFile(opts.SavePath)
                  .Then(imageFormatResult => new TagCloudSettings(opts.InputFiles,
                          opts.SavePath,
                          opts.BoringWordsPath,
                          opts.Width,
                          opts.Height,
                          Color.FromName(opts.BackgroundColor),
                          new FontFamily(opts.FontName),
                          opts.IgnoredPartsOfSpeech.Split(",", StringSplitOptions.RemoveEmptyEntries),
                          opts.GenerationAlgorithm,
                          opts.SplitType,
                          opts.ColorScheme,
                          Path.GetFullPath(Path.Combine("Resources", "mystem.exe")),
                          imageFormatResult));
              })
              .WithNotParsed(o => settings = Result.Fail<TagCloudSettings>("Wrong commandline argument."));
            return settings.Then(ValidateIsKnownColor)
                .Then(ValidateImageSize)
                .Then(ValidateMystemLocation)
                .Then(ValidateFontFamily);
        }

        private static Result<TagCloudSettings> ValidateIsKnownColor(TagCloudSettings settings)
        {
            return Validate(settings, settings => settings.backgroundColor.IsKnownColor, $"Unknown color {settings.backgroundColor.Name}");
        }

        private static Result<TagCloudSettings> ValidateFontFamily(TagCloudSettings settings)
        {
            return Validate(settings, 
                tagCloudSettings => FontFamily.Families.Any(fontFamily => fontFamily.Name == tagCloudSettings.fontFamily.Name),
                $"Unknown font {settings.fontFamily.Name}");
        }

        private static Result<TagCloudSettings> ValidateImageSize(TagCloudSettings settings)
        {
            return Validate(settings, settings => settings.widthOutputImage > 0 && settings.heightOutputImage > 0,
                $"The height and width of the resulting image must be greater than 0");
        }

        private static Result<TagCloudSettings> ValidateMystemLocation(TagCloudSettings settings)
        {
            return Validate(settings, settings => File.Exists(settings.pathToMystem),
                $"Mystem not found. Expected location is {settings.pathToMystem}");
        }

        private static Result<T> Validate<T>(T obj, Func<T, bool> predicate, string errorMessage)
        {
            return predicate(obj)
                ? Result.Ok(obj)
                : Result.Fail<T>(errorMessage);
        }
    }
}
