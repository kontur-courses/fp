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
            if (rectangle.IsIntersectOthersRectangles(_rectangles))
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
        var point = new Point
        {
            X = rectangle.GetCenter().X < _center.X ? ShiftPoint : -ShiftPoint,
            Y = rectangle.GetCenter().Y < _center.Y ? ShiftPoint : -ShiftPoint
        };
        while (rectangle.IsIntersectOthersRectangles(_rectangles) && _center.X != currentPosition.X 
                                                                  && _center.Y != currentPosition.Y)
        {
            currentPosition.X += currentPosition.X < _center.X ? ShiftPoint : -ShiftPoint;
            currentPosition.Y += currentPosition.Y < _center.Y ? ShiftPoint : -ShiftPoint;
            rectangle.Location = rectangle.Location.DecreasingCoordinate(point);
        }
        rectangle.Location = rectangle.Location.IncreasingCoordinate(point);

        return rectangle;
    }
}
