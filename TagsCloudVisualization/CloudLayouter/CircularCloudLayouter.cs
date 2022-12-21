using System.Drawing;
using System.Numerics;

namespace TagsCloudVisualization.CloudLayouter;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly ISpiralFormula _arithmeticSpiral;
    private readonly List<RectangleF> _rectangles;

    public IEnumerable<RectangleF> Rectangles => _rectangles;

    public CircularCloudLayouter(ISpiralFormula arithmeticSpiral)
    {
        _arithmeticSpiral = arithmeticSpiral;
        _rectangles = new List<RectangleF>();
    }

    public Result<RectangleF> PutNextRectangle(SizeF rectangleSize, LayoutOptions options)
    {
        if (options.SpiralStep <= 0)
            return Result.Fail<RectangleF>("SpiralStep must be greater than 0. Change LayoutOptions.");

        if (rectangleSize.IsEmpty)
            return Result.Fail<RectangleF>("Rectangle size empty. Check size of rectangles.");

        if (rectangleSize.Width <= 0.001 || rectangleSize.Height <= 0.001)
            return Result.Fail<RectangleF>("Rectangle size with or height must be greater than 0.001. Change LayoutOptions.");


        var currentLength = 0f;
        RectangleF rectangle;

        do
        {
            var nextPoint = _arithmeticSpiral.GetPoint(options.Center, currentLength);
            var centeredPoint = ShiftPointToRectangleCenter(nextPoint, rectangleSize);
            rectangle = new RectangleF(centeredPoint, rectangleSize);
            currentLength += options.SpiralStep;
        } while (_rectangles.Any(rect => rect.IntersectsWith(rectangle)));

        _rectangles.Add(rectangle);
        return rectangle;
    }

    private static PointF ShiftPointToRectangleCenter(PointF sourcePoint, SizeF rectSize)
    {
        var newX = sourcePoint.X - rectSize.Width / 2;
        var newY = sourcePoint.Y - rectSize.Height / 2;

        return new PointF(newX, newY);
    }

    public void Reset()
    {
        _rectangles.Clear();
    }
}