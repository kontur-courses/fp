using System.Drawing;

namespace TagCloud.PointGenerator;

public interface ICache
{
    float SafeGetParameter(SizeF size);
    void SafeUpdate(SizeF size, float radius);
}