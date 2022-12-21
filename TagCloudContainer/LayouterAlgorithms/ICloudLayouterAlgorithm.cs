using System.Drawing;

namespace TagCloudContainer.LayouterAlgorithms
{
    public interface ICloudLayouterAlgorithm
    {
        public Point PlaceNextWord(string word, int wordCount, int coefficient);
    }
}