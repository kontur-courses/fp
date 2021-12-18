using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultMonad;

namespace TagsCloudDrawer.Drawer
{
    public class Drawer : IDrawer
    {
        public Result<None> Draw(Graphics graphics, Size size, IEnumerable<IDrawable> drawables)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            var bounds = new Rectangle(Point.Empty, size);
            var shifted = drawables.Select(tag => tag.Shift(Size.Truncate(size / 2f)));
            foreach (var drawable in shifted)
            {
                if (!bounds.Contains(drawable.Bounds))
                    return Result.Fail<None>("Image cannot contain all rectangles");
                drawable.Draw(graphics);
            }

            return Result.Ok();
        }
    }
}