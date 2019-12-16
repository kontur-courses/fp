using System.Drawing;

namespace TagsCloudVisualization.Logic.Painter
{
    public static class SizeExtensions
    {
        public static Point GetCenter(this Size size)
        {
            return new Point(size.Width / 2, size.Height / 2);
        }
    }
}