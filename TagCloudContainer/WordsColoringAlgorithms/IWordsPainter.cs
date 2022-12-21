using System.Collections.Generic;
using System.Drawing;

namespace TagCloudContainer.WordsColoringAlgorithms
{
    public interface IWordsPainter
    {
        public Color[] GetColorsSequence(Dictionary<string, int> frequencyDictionary, Color startColor);
    }
}