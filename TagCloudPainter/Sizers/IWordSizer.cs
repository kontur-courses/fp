using System.Drawing;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Sizers;

public interface IWordSizer
{
    Result<Size> GetTagSize(string word, int count);
}