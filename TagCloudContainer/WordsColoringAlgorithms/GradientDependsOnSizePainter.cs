using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudContainer.TaskResult;

namespace TagCloudContainer.WordsColoringAlgorithms
{
    public class GradientDependsOnSizePainter : IWordsPainter
    {
        public Dictionary<string, Color> GetWordColorDictionary(Dictionary<string, int> frequencyDictionary,
            Color startColor)
        {
            var result = new Dictionary<string, Color>();
            var maxWordCount = frequencyDictionary.Values.Max();
            var resultA = startColor.A / maxWordCount;
            foreach (var pair in frequencyDictionary)
            {
                var wordCount = pair.Value;
                result[pair.Key] = Color.FromArgb(resultA * wordCount, startColor.R, startColor.G, startColor.B);
            }

            return result;
        }
    }
}