using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloudContainer.Result;

namespace TagCloudContainer.WordsColoringAlgorithms
{
    public class DefaultWordsPainter : IWordsPainter
    {
        public Result<Color[]> GetColorsSequence(Dictionary<string, int> frequencyDictionary, Color startColor)
        {
            var wordsCount = frequencyDictionary.Count;
            if (wordsCount == 0 || startColor.IsEmpty)
                return new Result<Color[]>("Brush color mistake");
            var colors = new Color[wordsCount];
            for (var i = 0; i < wordsCount; i++)
                colors[i] = startColor;
            return new Result<Color[]>(null, colors);
        }
    }
}