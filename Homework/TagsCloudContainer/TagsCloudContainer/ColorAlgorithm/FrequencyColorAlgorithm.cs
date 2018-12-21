using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudResult;

namespace TagsCloudContainer.ColorAlgorithm
{
    public class FrequencyColorAlgorithm : IColorAlgorithm
    {
        public Result<Color> GetColor(Dictionary<string, int> words = null, string word = "")
        {
            if (words == null || !words.ContainsKey(word))
                return Result.Ok<Color>(Color.Black);

            var maxRepeatCount = words.Max(dWord => dWord.Value);
            var hue = 255 - (int)(255 * words[word] / maxRepeatCount);

            return Result.Ok<Color>(Color.FromArgb(hue, hue, hue));
        }
    }
}