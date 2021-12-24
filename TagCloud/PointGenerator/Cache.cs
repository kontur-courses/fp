using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("TagCloudTests")]
namespace TagCloud.PointGenerator;
internal class Cache : ICache
{
    private readonly Dictionary<SizeF, float> sizeToCircleParameter = new();

    public float SafeGet(SizeF size)
    {
        if (!sizeToCircleParameter.ContainsKey(size))
            sizeToCircleParameter[size] = 0;
        return sizeToCircleParameter[size];
    }

    public void UpdateOrAdd(SizeF size, float radius)
    {
        sizeToCircleParameter[size] = radius;
    }
}