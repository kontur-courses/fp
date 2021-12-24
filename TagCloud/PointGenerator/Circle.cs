using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.PointGenerator;
public class Circle : IPointGenerator
{
    private float spiralPitch;
    private readonly float anglePitch;
    private readonly double pitchCoefficient;
    private readonly ICache cache;

    public Circle(float anglePitch, double densityCoefficient, ICache cache)
    {
        this.anglePitch = anglePitch;
        this.cache = cache;
        pitchCoefficient = 20 * densityCoefficient * densityCoefficient;
    }

    public IEnumerable<PointF> GetPoints(SizeF size)
    {
        spiralPitch = (float)(Math.Min(size.Height, size.Width) / pitchCoefficient);
        foreach (var polar in ArchimedeanSpiral.GetArchimedeanSpiral(cache.SafeGet(size),
                     anglePitch, spiralPitch))
        {
            cache.UpdateOrAdd(size, polar.Angle);
            var cartesianPoint = polar.ToCartesian();
            yield return new PointF(cartesianPoint.X, cartesianPoint.Y);
        }
    }

    public static Circle GetDefault()
    {
        return new Circle(0.1f, 0.9, new Cache());
    }
}