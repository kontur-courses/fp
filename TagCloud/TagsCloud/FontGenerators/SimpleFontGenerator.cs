using System;
using System.Drawing;
using TagsCloud.ErrorHandling;
using TagsCloud.Interfaces;

namespace TagsCloud.FontGenerators
{
    public class SimpleFontGenerator : IFontSettingsGenerator
    {
        private readonly FontFamily fontFamily;
        private readonly int maxFontSize;
        private readonly int minFontSize;

        public SimpleFontGenerator(TagCloudSettings settings, int maxFontSize = 128, int minFontSize = 40)
        {
            fontFamily = settings.fontFamily;
            this.maxFontSize = maxFontSize;
            this.minFontSize = minFontSize;
        }

        public Result<FontSettings> GetFontSizeForCurrentWord((string word, int frequency) wordFrequency,
            int positionByFrequency, int countWords)
        {
            if (countWords == 0)
                return Result.Fail<FontSettings>("The count of words cannot be zero");
            var fontSize = maxFontSize * ((float) (countWords - positionByFrequency + 1) / countWords);
            fontSize = Math.Max(fontSize, minFontSize);
            return Result.Of(() => new FontSettings(fontFamily, fontSize));
        }
    }
}