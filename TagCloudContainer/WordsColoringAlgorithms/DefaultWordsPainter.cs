using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloudContainer.WordsColoringAlgorithms
{
    public class DefaultWordsPainter : IWordsPainter
    {
        public Dictionary<string, Color> GetWordColorDictionary(Dictionary<string, int> frequencyDictionary,
            Color startColor)
        {
            var result = new Dictionary<string, Color>();
            foreach (var pair in frequencyDictionary)
                result[pair.Key] = startColor;
            return result;
        }
    }
}