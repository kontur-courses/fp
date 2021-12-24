using System.Drawing;

namespace TagCloud.PointGenerator;

public interface ICache
{
    float SafeGet(SizeF size);
    void UpdateOrAdd(SizeF size, float radius);
}