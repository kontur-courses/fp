using System.Drawing;

namespace TagsCloud.Common
{
    public interface ISpiralFactory
    {
        ISpiral Create(Point center);
    }
}