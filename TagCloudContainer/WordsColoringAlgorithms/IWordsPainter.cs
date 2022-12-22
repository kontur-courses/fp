using System.Collections.Generic;
using System.Drawing;
using TagCloudContainer.Result;

namespace TagCloudContainer.WordsColoringAlgorithms
{
    public interface IWordsPainter
    {
        public Result<Color[]> GetColorsSequence(Dictionary<string, int> frequencyDictionary, Color startColor);
    }
}