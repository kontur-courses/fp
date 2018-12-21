using System.Collections.Generic;
using System.Drawing;
using TagsCloudResult;

namespace TagsCloudContainer.ColorAlgorithm
{
    public interface IColorAlgorithm
    {
        Result<Color> GetColor(Dictionary<string, int> words = null, string word = "");
    }
}
