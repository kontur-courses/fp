using System.Drawing;

namespace TagCloud.FigurePatterns
{
    public interface IFigurePatternPointProvider
    {
        Point GetNextPoint();
        void Restart();
    }
}