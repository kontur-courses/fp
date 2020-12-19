using System.Drawing;

namespace TagsCloud.Common
{
    public interface ICloudLayouterFactory
    {
        ICircularCloudLayouter CreateCircularLayouter(Point center);
    }
}