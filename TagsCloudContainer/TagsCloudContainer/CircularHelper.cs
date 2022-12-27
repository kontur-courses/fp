using System.Drawing;

namespace TagsCloudContainer;

public static class CircularHelper
{
    /// <summary>
    ///     Provide archimedes spiral
    /// </summary>
    /// <param name="polarStepK"></param>
    /// <param name="angleStep"></param>
    /// <param name="center"></param>
    /// <param name="startAngle"></param>
    /// <param name="maximum">maximum point count, if equal max then it is endless</param>
    /// <returns></returns>
    public static IEnumerable<Point> EnumeratePointsInArchimedesSpiral(float polarStepK,
        float angleStep,
        Point center,
        float startAngle = 0f,
        ulong maximum = ulong.MaxValue - 1)
    {
        var current = new PointF(center.X, center.Y);
        var centerX = (float)center.X;
        var centerY = (float)center.Y;
        var angle = startAngle;

        while (maximum == ulong.MaxValue || maximum-- > 0)
        {
            yield return Point.Round(current);

            var p = polarStepK * angle;
            var x = centerX + p * MathF.Cos(angle);
            var y = centerY + p * MathF.Sin(angle);

            current = new(x, y);
            angle += angleStep;
        }
    }
}