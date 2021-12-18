using System.Collections.Generic;
using System.Drawing;
using ResultMonad;

namespace TagsCloudDrawer.Drawer
{
    public interface IDrawer
    {
        Result<None> Draw(Graphics graphics, Size size, IEnumerable<IDrawable> drawables);
    }
}