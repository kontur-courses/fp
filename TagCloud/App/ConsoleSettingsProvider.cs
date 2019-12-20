using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;
using TagCloud.Infrastructure;
using TagCloud.WordsProcessing;

namespace TagCloud.App
{
    public class ConsoleSettingsProvider : ISettingsProvider
    {
        private readonly Options options;
        private readonly PictureConfig pictureConfig;
        private readonly IFileInfoProvider fileInfoProvider;
        private AppSettings savedSettings;

        public ConsoleSettingsProvider(Options options, PictureConfig pictureConfig, IFileInfoProvider fileInfoProvider)
        {
            this.options = options;
            this.pictureConfig = pictureConfig;
            this.fileInfoProvider = fileInfoProvider;
        }

        public Result<AppSettings> GetSettings()
        {
            if (savedSettings != null)
                return savedSettings;
            savedSettings = new AppSettings {PictureConfig = pictureConfig, OutputFilePath = options.OutputFilePath};
            return savedSettings.AsResult()
                .Then(SetFileInfo)
                .RefineError("Unable to get file info")
                .Then(SetSize)
                .Then(SetFontFamily)
                .Then(SetBackground)
                .Then(SetColors)
                .Then(SetWordClasses)
                .Then(SetWordPainterAlgorithmName);
        }

        private Result<AppSettings> SetFileInfo(AppSettings settings)
        {
            var fileInfoResult = fileInfoProvider.GetFileInfo(options.InputFilePath);
            if (!fileInfoResult.IsSuccess)
                return Result.Fail<AppSettings>(fileInfoResult.Error);
            settings.InputFileInfo = fileInfoResult.GetValueOrThrow();
            return settings;
        }

        private Result<AppSettings> SetSize(AppSettings settings)
        {
            if (options.Width == 0 || options.Height == 0)
            {
                settings.PictureConfig.Size = PictureConfig.MinSize;
                return settings;
            }
            if (options.Width < PictureConfig.MinSize.Width || options.Height < PictureConfig.MinSize.Height)
                return Result
                    .Fail<AppSettings>($"Size {options.Width}x{options.Height} is too small for tag cloud");
            settings.PictureConfig.Size = new Size(options.Width, options.Height);
            return settings;
        }

        private Result<AppSettings> SetFontFamily(AppSettings settings)
        {
            if (options.FontFamily == null)
                return settings;
            FontFamily fontFamily;
            try
            {
                fontFamily = new FontFamily(options.FontFamily);
            }
            catch (ArgumentException e)
            {
                return Result.Fail<AppSettings>(e.Message)
                    .RefineError($"Font {options.FontFamily} doesn't exist");
            }
            settings.PictureConfig.FontFamily = fontFamily;
            return settings;

        }

        private Result<AppSettings> SetBackground(AppSettings settings)
        {
            if (options.Background == null)
                return settings;
            var colorResult = GetColorFromName(options.Background);
            if (!colorResult.IsSuccess)
                return Result.Fail<AppSettings>(colorResult.Error);
            settings.PictureConfig.Palette.BackgroundColor = colorResult.GetValueOrThrow();
            return settings;
        }

        private Result<AppSettings> SetColors(AppSettings settings)
        {
            var inputColors = options.WordsColors.ToList();
            if (inputColors.Count == 0)
                return settings;
            var colors = new List<Color>();
            foreach (var colorResult in inputColors.Select(GetColorFromName))
            {
                if (!colorResult.IsSuccess)
                    return Result.Fail<AppSettings>(colorResult.Error);
                colors.Add(colorResult.GetValueOrThrow());
            }
            settings.PictureConfig.Palette.WordsColors = colors.ToArray();
            return settings;
        }

        private Result<AppSettings> SetWordClasses(AppSettings settings)
        {
            var wordClasses = options.WordClasses.ToList();
            if (wordClasses.Count == 0)
            {
                settings.WordClassSettings = new WordClassSettings();
                return settings;
            }
            var wordClassesResult = ParseWordClasses(wordClasses);
            if (!wordClassesResult.IsSuccess)
                return Result.Fail<AppSettings>(wordClassesResult.Error);
            settings.WordClassSettings = new WordClassSettings(wordClassesResult.GetValueOrThrow(), false);
            return settings;
        }

        private Result<AppSettings> SetWordPainterAlgorithmName(AppSettings settings)
        {
            settings.WordPainterAlgorithmName = options.WordPainter ?? "index";
            return settings;
        }

        private static Result<Color> GetColorFromName(string name)
        {
            var color = Color.FromName(name);
            return !color.IsKnownColor ? Result.Fail<Color>($"Unknown color: {name}") : color;
        }

        private static Result<HashSet<WordClass>> ParseWordClasses(IEnumerable<string> inputWordClasses)
        {
            var wordClasses = new HashSet<WordClass>();
            foreach (var wordClass in inputWordClasses)
            {
                if (Enum.TryParse(wordClass, true, out WordClass result))
                    wordClasses.Add(result);
                else
                    return Result.Fail<HashSet<WordClass>>($"Unknown word class: {wordClass}");
            }
            return wordClasses;
        }
    }
}
