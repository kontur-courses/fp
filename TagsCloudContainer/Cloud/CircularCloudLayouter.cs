using System.Drawing;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.Cloud;

public class CircularCloudLayouter
{
    public List<Rectangle> _rectangles;
    private readonly SpiralFunction spiralFunction;
    private readonly Point _center;
    private const int ShiftPoint = 1;

    public CircularCloudLayouter(Point center)
    {
        _center = center;
        _rectangles = new List<Rectangle>();
        spiralFunction = new SpiralFunction(center, 2);
    }

    public Result<Rectangle> PutNextRectangle(Size sizeRectangle)
    {
        if (sizeRectangle.Width < 0 || sizeRectangle.Height < 0 || sizeRectangle.IsEmpty)
        {
            return Result.Fail<Rectangle>("Высота и ширина должны быть больше 0");
        }

        Rectangle rectangle;

        while(true)
        {
            var nextPoint = spiralFunction.GetNextPoint();
            rectangle = new Rectangle(nextPoint, sizeRectangle);
            if (!rectangle.IsIntersecting(_rectangles))
            {
                break;  
            }
        }
        rectangle = MoveRectangleAxis(rectangle);
        _rectangles.Add(rectangle);
        
        return rectangle.Ok();
    }
    
    private Rectangle MoveRectangleAxis(Rectangle rectangle)
    {
        var currentPosition = rectangle.GetCenter();
        var point = currentPosition.GetShiftRectangle(_center, ShiftPoint);
        
        while (!IsSuitable(rectangle, currentPosition))
        {
            currentPosition += new Size(currentPosition.GetShiftRectangle(_center, ShiftPoint));
            rectangle.Location = rectangle.Location.DecreasingCoordinate(point);
        }
        rectangle.Location = rectangle.Location.IncreasingCoordinate(point);

        return rectangle;
    }

    private bool IsSuitable(Rectangle rectangle, Point currentPosition) =>
        rectangle.IsIntersecting(_rectangles) || _center.X == currentPosition.X || _center.Y == currentPosition.Y;
}
