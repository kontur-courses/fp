using System.Drawing;

namespace TagCloud.Creators
{
    public interface ICreator<out T>
    {
        T Place(Point point, Size size);
    }
}