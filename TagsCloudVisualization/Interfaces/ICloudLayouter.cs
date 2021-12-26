using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
    public interface ICloudLayouter
    {
        Result<Rectangle> PutNextWord(string word, Font font, Graphics graphics);
        Result<List<Rectangle>> PutWords(IEnumerable<string> words, Font font, Graphics graphics);
        Result<Rectangle[]> GetPutRectangles();
        Result<Point> GetCenter();
    }
}