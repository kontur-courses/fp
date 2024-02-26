using System.Drawing;

namespace TagsCloudContainer;

public static class RectangleExtension
{
    public static Point DecreasingCoordinate(this Point selfPoint, Point otherPoint) =>
        new(selfPoint.X + otherPoint.X, selfPoint.Y + otherPoint.Y);
    
    public static Point IncreasingCoordinate(this Point selfPoint, Point otherPoint) =>
        new(selfPoint.X - otherPoint.X, selfPoint.Y - otherPoint.Y);
    
    public static Point GetCenter(this Rectangle rectangle) =>
        new(rectangle.Location.X + rectangle.Width / 2, rectangle.Location.Y + rectangle.Height / 2);
    
    public static bool IsIntersecting(this Rectangle rectangle, List<Rectangle> rectangles) => 
        rectangles.Any(rectangle.IntersectsWith);
    
    public static Point GetShiftRectangle(this Point p1, Point p2, int shift)
    {
        return new Point
        {
            X = p1.X < p2.X ? shift : -shift,
            Y = p1.Y < p2.Y ? shift : -shift,
        };
    }
    
    public static bool IsRectangleOutOfBitmap(Rectangle rectangle, RectangleF bitmapBounds)
    {
        return rectangle.Left < 0 || rectangle.Top < 0 
                                  || rectangle.Right > bitmapBounds.Width 
                                  || rectangle.Bottom > bitmapBounds.Height;
    }
    
    public static Bitmap GetBounds(List<Rectangle> rectangles)
    {
        var bounds = rectangles
            .Aggregate(
                (int.MaxValue, int.MaxValue, int.MinValue, int.MinValue), 
                (prev, r) => GetMaxBounds(r, prev));
        
        var width = bounds.Item3 - bounds.Item1;
        var height = bounds.Item4 - bounds.Item2;
        
        return new Bitmap(width * 2, height * 2);
    }
    
    private static (int left, int top, int right, int bottom) GetMaxBounds(
        Rectangle rectangle,
        (int left, int top, int right, int bottom) current
    )
    {
        return (
            Math.Min(current.left, rectangle.Left),
            Math.Min(current.top, rectangle.Top),
            Math.Max(current.right, rectangle.Right),
            Math.Max(current.bottom, rectangle.Bottom));
    }
    
}
