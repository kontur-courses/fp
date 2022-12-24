using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloudContainer.TaskResult;

namespace TagCloudContainer.WordsColoringAlgorithms
{
    public class GradientWordsPainter : IWordsPainter
    {
        public Dictionary<string, Color> GetWordColorDictionary(Dictionary<string, int> frequencyDictionary,
            Color startColor)
        {
            var result = new Dictionary<string, Color>();
            foreach (var pair in frequencyDictionary)
            {
                var resultA = startColor.A - 10 > 0 ? startColor.A - 10 : 0;
                startColor = Color.FromArgb(resultA, startColor.R, startColor.G, startColor.B);
                result[pair.Key] = startColor;
            }

            return result;
        }
    }
}