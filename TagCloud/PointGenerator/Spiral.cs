using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.PointGenerator;

public class Spiral : IPointGenerator
{
    private readonly float spiralPitch;
    private readonly float anglePitch;
    public PointF Center { get; }
    private readonly ICache cache;

    public Spiral(float anglePitch, float spiralPitch, ICache cache)
    {
        this.anglePitch = anglePitch;
        this.cache = cache;
        this.spiralPitch = spiralPitch;
    }

    public IEnumerable<PointF> GetPoints(SizeF size)
    {
        foreach (var polar in ArchimedeanSpiral.GetArchimedeanSpiral(cache.SafeGet(size), anglePitch, spiralPitch))
        {
            cache.UpdateOrAdd(size, polar.Angle);
            var cartesian = polar.ToCartesian();
            yield return new PointF(cartesian.X + Center.X, cartesian.Y + Center.Y);
        }
    }

    public static Spiral GetDefaultSpiral()
    {
        return new Spiral(2.5f, 25f, new Cache());
    }
}