using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudContainer.Result;

namespace TagCloudContainer.WordsColoringAlgorithms
{
    public class GradientDependsOnSizePainter : IWordsPainter
    {
        public Result<Color[]> GetColorsSequence(Dictionary<string, int> frequencyDictionary, Color startColor)
        {
            if (frequencyDictionary.Count == 0)
                return new Result<Color[]>("Brush color mistake");
            var maxWordCount = frequencyDictionary.Values.Max();
            var resultA = startColor.A / maxWordCount;
            var colors = new Color[frequencyDictionary.Count];
            var counter = 0;
            foreach (var wordCount in frequencyDictionary.Values)
            {
                colors[counter] = Color.FromArgb(resultA * wordCount, startColor.R, startColor.G, startColor.B);
                counter++;
            }

            return new Result<Color[]>(null, colors);
        }
    }
}