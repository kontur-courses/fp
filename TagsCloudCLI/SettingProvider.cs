using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ResultMonad;
using TagsCloudVisualization.Settings;

namespace TagsCloudCLI
{
    internal class SettingProvider
    {
        public GeneralSettings GetSettings(Options options)
        {
            var fontSettings = new FontSettings(options.MaxFontSize, options.FontFamilyName);
            var saverSettings = new SaverSetting(options.Directory, options.ImageName);
            var wordsPreprocessorSettings =
                new WordsPreprocessorSettings(GetBoringWordsFromFile(options.PathToBoringWords).GetValueOrThrow());
            var reader = new ReaderSettings(options.FileWithWords);
            var drawerSettings = new DrawerSettings(Color.FromName(options.TagColor));

            return new GeneralSettings(fontSettings, saverSettings, wordsPreprocessorSettings, reader, drawerSettings,
                new Point(0, 0));
        }
        
        private static Result<IEnumerable<string>> GetBoringWordsFromFile(string filename)
        {
            return filename.AsResult()
                .Validate(File.Exists, $"No such file {filename}")
                .Then(File.ReadLines);
        }
    }
}