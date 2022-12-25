using System.Drawing;
using TagsCloudVisualization.ColorGenerator;

namespace TagsCloudVisualization.Drawer;

public class RectangleImage : IDrawImage
{
    private readonly IColorGenerator colorGenerator;

    public RectangleImage(Rectangle rectangle, IColorGenerator colorGenerator)
    {
        this.colorGenerator = colorGenerator;
        Bounds = rectangle;
    }

    public Rectangle Bounds { get; }

    public Result<None> Draw(Graphics graphics)
    {
        return Result.Of(() => graphics.DrawRectangle(new Pen(colorGenerator.Generate()), Bounds),
            "Can't draw rectangle");
    }

    public IDrawImage Offset(Size size)
    {
        return new RectangleImage(new Rectangle(Bounds.Location + size, Bounds.Size), colorGenerator);
    }
}