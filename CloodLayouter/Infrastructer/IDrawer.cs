using System.Drawing;
using ResultOf;

namespace CloodLayouter.Infrastructer
{
    public interface IDrawer
    {
        Result<Bitmap> Draw();
    }
}