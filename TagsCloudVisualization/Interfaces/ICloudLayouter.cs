#region

using System.Collections.Generic;
using System.Drawing;

#endregion

namespace TagsCloudVisualization.Interfaces
{
    public interface ICloudLayouter
    {
        Result<Rectangle> PutNextWord(string word, Font font, Graphics graphics);
        Result<IEnumerable<Rectangle>> PutWords(IEnumerable<string> words, Font font, Graphics graphics);
        Result<Rectangle[]> GetPutRectangles();
        Result<Point> GetCenter();
    }
}