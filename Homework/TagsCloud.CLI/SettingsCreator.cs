using System;
using System.Drawing;
using System.IO;
using TagsCloud.Visualization;
using TagsCloud.Visualization.ContainerVisitor;
using TagsCloud.Visualization.Drawers;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.FontFactory;
using TagsCloud.Visualization.ImagesSavers;
using TagsCloud.Visualization.Utils;
using TagsCloud.Words.Options;

namespace TagsCloud.Words
{
    public static class SettingsCreator
    {
        public static Result<TagsCloudModuleSettings> CreateFrom(CreateCloudCommand options) =>
            from fontSettings in ParseFont(options)
            from saveSettings in ParseSaveSettings(options)
            from layouterType in ParseLayouter(options.Algorithm)
            select new TagsCloudModuleSettings
            {
                Center = Point.Empty,
                InputWordsFile = options.WordsFile,
                BoringWordsFile = options.BoringWordsFile,
                FontSettings = fontSettings,
                SaveSettings = saveSettings,
                LayouterType = layouterType,
                LayoutVisitor = CreateDrawerVisitorFromName(options.Color)
            };

        private static Result<FontSettings> ParseFont(CreateCloudCommand options) =>
            from fontStyle in ParseFontStyle(options.FontStyle)
            from fontName in ParseFontName(options.FamilyName)
            select new FontSettings
            {
                FamilyName = fontName,
                MaxSize = options.MaxSize,
                FontStyle = fontStyle
            };

        private static Result<SaveSettings> ParseSaveSettings(CreateCloudCommand options) =>
            new SaveSettings
            {
                OutputDirectory = options.OutputDirectory ?? GetDirectoryForSavingExamples(),
                OutputFileName = options.OutputFile,
                Extension = options.Extension
            };

        private static Result<FontStyle> ParseFontStyle(string fontStyleName)
        {
            return Enum.TryParse<FontStyle>(fontStyleName, true, out var style) switch
            {
                true => style,
                false => Result.Fail<FontStyle>($"Unknown font style {fontStyleName}")
            };
        }

        private static Result<string> ParseFontName(string fontName)
        {
            using var testFont = new Font(fontName, 8);
            return fontName.Equals(testFont.Name, StringComparison.InvariantCultureIgnoreCase)
                ? fontName
                : Result.Fail<string>($"font with name {fontName} doesn't exists");
        }

        private static Result<Type> ParseLayouter(string layouterName)
        {
            return layouterName switch
            {
                "circular" => typeof(CircularCloudLayouter),
                _ => Result.Fail<Type>($"Layouter {layouterName} is not defined")
            };
        }

        private static IContainerVisitor CreateDrawerVisitorFromName(string textColor)
        {
            if (textColor == "random")
                return new RandomColorDrawerVisitor();

            var color = Color.FromName(textColor);
            return new ConcreteColorDrawerVisitor(new ImageSettings {Color = color});
        }

        private static string GetDirectoryForSavingExamples()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Examples");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }
}