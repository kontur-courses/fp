using System.Drawing;

namespace TagsCloudVisualization.Drawer;

public interface IDrawImage
{
    Rectangle Bounds { get; }
    Result<None> Draw(Graphics graphics);
    IDrawImage Offset(Size size);
}