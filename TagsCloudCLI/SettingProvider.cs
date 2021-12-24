using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ResultMonad;
using ResultMonad.Extensions;
using TagsCloudVisualization.Settings;

namespace TagsCloudCLI
{
    internal class SettingProvider
    { 
        public Result<GeneralSettings> GetSettings(Options options) =>
            from font in ParseFont(options.FontFamilyName, options.MaxFontSize)
            from preprocessor in ParseWordsPreprocessorSettings(options.PathToBoringWords)
            from drawer in ParseDrawerSettings(options.TagColor)
            select new GeneralSettings(font, new SaverSetting(options.Directory, options.ImageName), preprocessor,
                new ReaderSettings(options.FileWithWords), drawer, Point.Empty);

        private static Result<FontSettings> ParseFont(string fontFamilyName, int maxFontSize)
        {
            using var testFont = new Font(fontFamilyName, 8);
            return testFont.AsResult()
                .Validate(font => fontFamilyName.Equals(font.Name, StringComparison.InvariantCultureIgnoreCase),
                    $"Font name {fontFamilyName} doesn't exists")
                .Validate(maxFontSize > 0, $"Font size must be positive, not {maxFontSize}")
                .Then(font => new FontSettings(maxFontSize, font.Name));
        }

        private static Result<WordsPreprocessorSettings> ParseWordsPreprocessorSettings(string pathToBoringWords) =>
            GetBoringWordsFromFile(pathToBoringWords)
                .Then(path => new WordsPreprocessorSettings(path));
        
        private static Result<IEnumerable<string>> GetBoringWordsFromFile(string filename) =>
            filename.AsResult()
                .Validate(File.Exists, $"No such file {filename}")
                .Then(File.ReadLines);

        private static Result<DrawerSettings> ParseDrawerSettings(string colorName) => 
            colorName.AsResult()
                .Then(Color.FromName)
                .Validate(color => color.IsKnownColor, $"Unknown color {colorName}")
                .Then(color => new DrawerSettings(color));
    }
}