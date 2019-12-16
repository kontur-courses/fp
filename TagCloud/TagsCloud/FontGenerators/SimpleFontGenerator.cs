﻿using TagsCloud.Interfaces;
using TagsCloud.ErrorHandling;
using System;

namespace TagsCloud.FontGenerators
{
    public class SimpleFontGenerator : IFontSettingsGenerator
    {
        private readonly int maxFontSize;
        private readonly int minFontSize;
        private readonly string fontName;

        public SimpleFontGenerator(TagCloudSettings fontName, int maxFontSize=128, int minFontSize=40)
        {
            this.fontName = fontName.fontName;
            this.maxFontSize = maxFontSize;
            this.minFontSize = minFontSize;
        }

        public Result<FontSettings> GetFontSizeForCurrentWord((string word, int frequency) wordFrequency, int positionByFrequency, int countWords)
        {
            float fontSize =  maxFontSize * ((float)(countWords - positionByFrequency + 1) / countWords);
            fontSize = Math.Max(fontSize, minFontSize);
            return Result.Of(() => new FontSettings(fontName, fontSize));
        }
    }
}
