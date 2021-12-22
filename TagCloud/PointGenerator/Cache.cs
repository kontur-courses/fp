using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.PointGenerator
{
    public class Cache : ICache
    {
        private readonly Dictionary<SizeF, float> sizeToCircleParameter = new();

        public float SafeGetParameter(SizeF size)
        {
            if (!sizeToCircleParameter.ContainsKey(size))
                sizeToCircleParameter[size] = 0;
            return sizeToCircleParameter[size];
        }

        public void SafeUpdate(SizeF size, float radius)
        {
            sizeToCircleParameter[size] = radius;
        }
    }
}