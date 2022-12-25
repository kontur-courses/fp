using System.Drawing;

namespace TagsCloudVisualization;

public class TextLabel
{
    public string Content { get; set; }
    public Font Font { get; set; }
    public Rectangle Rectangle { get; set; }

    public IEnumerable<Point> GetСornersPositions()
    {
        yield return new Point(Rectangle.Top, Rectangle.Right);
        yield return new Point(Rectangle.Top, Rectangle.Left);
        yield return new Point(Rectangle.Bottom, Rectangle.Right);
        yield return new Point(Rectangle.Bottom, Rectangle.Left);
    }
}