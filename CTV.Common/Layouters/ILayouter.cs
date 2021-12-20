using System.Drawing;

namespace CTV.Common.Layouters
{
    public interface ILayouter
    {
        Point Center { get; set; }
        Rectangle PutNextRectangle(Size size);
    }
}