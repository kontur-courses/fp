using System.Collections.Generic;
using System.Drawing;
using TagsCloudResult;

namespace TagsCloudContainer.ColorAlgorithm
{
    public class StaticColorAlgorithm : IColorAlgorithm
    {
        public Result<Color> GetColor(Dictionary<string, int> words = null, string word = "")
        {
            return Color.Black;
        }
    }
}