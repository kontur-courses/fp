using System.Drawing;
using TagCloudContainer.Result;

namespace TagCloudContainer.LayouterAlgorithms
{
    public interface ICloudLayouterAlgorithm
    {
        public Result<Point> PlaceNextWord(string word, int wordCount, int coefficient);
    }
}