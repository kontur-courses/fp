using System.Drawing;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Interfaces
{
    public interface IColorScheme
    {
        Result<Color> GetColorForCurrentWord((string word, int frequency) wordFrequency, int positionByFrequency,
            int countWords);
    }
}