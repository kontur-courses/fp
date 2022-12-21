using System.Drawing;
using ResultOf;

namespace TagCloud.Common.Layouter;

public class CircularCloudLayouter : ICloudLayouter
{
    public Point Center { get; }
    private readonly List<Rectangle> rectangles;
    private Point currentPoint;
    private double angle;

    public CircularCloudLayouter(Point center)
    {
        Center = center;
        rectangles = new List<Rectangle>();
        currentPoint = center;
    }

    public Result<Rectangle> PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
        {
            return Result.Fail<Rectangle>("Can't layout rectangle with negative size");
        }

        var rectangle = new Rectangle(currentPoint, rectangleSize);
        while (!CanBePlaced(rectangle))
        {
            CalculateNewPoint();
            rectangle.Location = currentPoint;
        }

        rectangles.Add(rectangle);
        return rectangle.AsResult();
    }

    public IEnumerable<Rectangle> GetRectanglesLayout()
    {
        return rectangles.ToArray();
    }

    public void ClearRectanglesLayout()
    {
        rectangles.Clear();
        angle = 0.0;
    }

    private void CalculateNewPoint()
    {
        currentPoint.X = (int)(Math.Cos(angle) * angle + Center.X);
        currentPoint.Y = (int)(Math.Sin(angle) * angle + Center.Y);
        angle += 0.001;
    }

    private bool CanBePlaced(Rectangle rectangle)
    {
        return rectangles.All(rec => !rec.IntersectsWith(rectangle));
    }
}