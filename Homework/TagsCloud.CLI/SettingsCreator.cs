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

namespace TagsCloud.Words
{
    public class SettingsCreator
    {
        public Result<TagsCloudModuleSettings> Parse(Options options) =>
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

        private Result<FontSettings> ParseFont(Options options) =>
            from fontStyle in ParseFontStyle(options.FontStyle)
            from fontName in ParseFontName(options.FamilyName)
            select new FontSettings
            {
                FamilyName = fontName,
                MaxSize = options.MaxSize,
                FontStyle = fontStyle
            };

        private Result<SaveSettings> ParseSaveSettings(Options options) =>
            new SaveSettings
            {
                OutputDirectory = options.OutputDirectory ?? GetDirectoryForSavingExamples(),
                OutputFileName = options.OutputFile,
                Extension = options.Extension
            };

        private Result<FontStyle> ParseFontStyle(string fontStyleName)
        {
            return Enum.TryParse<FontStyle>(fontStyleName, true, out var style) switch
            {
                true => style,
                false => Result.Fail<FontStyle>($"Unknown font style {fontStyleName}")
            };
        }

        private Result<string> ParseFontName(string fontName)
        {
            using var testFont = new Font(fontName, 8);
            return fontName.Equals(testFont.Name, StringComparison.InvariantCultureIgnoreCase)
                ? fontName
                : Result.Fail<string>($"font with name {fontName} doesn't exists");
        }

        private Result<Type> ParseLayouter(string layouterName)
        {
            return layouterName switch
            {
                "circular" => typeof(CircularCloudLayouter),
                _ => Result.Fail<Type>($"Layouter {layouterName} is not defined")
            };
        }

        private IContainerVisitor CreateDrawerVisitorFromName(string textColor)
        {
            if (textColor == "random")
                return new RandomColorDrawerVisitor();

            var color = Color.FromName(textColor);
            return new ConcreteColorDrawerVisitor(new ImageSettings {Color = color});
        }

        private string GetDirectoryForSavingExamples()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Examples");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }
}