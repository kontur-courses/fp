using System.Drawing;

namespace TagCloud;

public class CircularCloudLayouter
{
    private List<Rectangle> rectangles;
    private ICloudShaper shaper;

    public IReadOnlyList<Rectangle> Rectangles => rectangles;

    public CircularCloudLayouter(ICloudShaper shaper)
    {
        rectangles = new List<Rectangle>();
        this.shaper = shaper;
    }

    public Result<Rectangle> PutNextRectangle(Size size)
    {
        if (size.Width <= 0)
            return Result.Fail<Rectangle>("Size width must be positive number");
        if (size.Height <= 0)
            return Result.Fail<Rectangle>("Size height must be positive number");
        
        Rectangle rectangle = Rectangle.Empty;
        foreach (var point in shaper.GetPossiblePoints())
        {
            rectangle = new Rectangle(point, size);
            if (!Rectangles.Any(rect => rect.IntersectsWith(rectangle)))
                break;
        }
        
        rectangles.Add(rectangle);
        return rectangle;
    }
}