using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloudContainer.Result;

namespace TagCloudContainer.WordsColoringAlgorithms
{
    public class GradientWordsPainter : IWordsPainter
    {
        public Result<Color[]> GetColorsSequence(Dictionary<string, int> frequencyDictionary, Color startColor)
        {
            if (frequencyDictionary.Count == 0)
                return new Result<Color[]>("Brush color mistake");
            var counter = 0;
            var colors = new Color[frequencyDictionary.Count];
            while (counter < frequencyDictionary.Count)
            {
                var resultA = startColor.A - 10 > 0 ? startColor.A - 10 : 0;
                startColor = Color.FromArgb(resultA, startColor.R, startColor.G, startColor.B);
                colors[counter] = startColor;
                counter++;
            }

            return new Result<Color[]>(null, colors);
        }
    }
}