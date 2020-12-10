using System.Drawing;

namespace TagsCloud.TextProcessing.WordsMeasurer
{
    public interface IWordMeasurer
    {
        Size GetWordSize(string word, Font font);
    }
}