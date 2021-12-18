using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultMonad;

namespace TagsCloudDrawer.Drawer
{
    public class Drawer : IDrawer
    {
        public Result<None> Draw(Graphics graphics, Size size, IEnumerable<Result<IDrawable>> drawables)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            var bounds = new Rectangle(Point.Empty, size);
            return drawables
                .Select(tag => tag.Then(t => t.Shift(Size.Truncate(size / 2f))))
                .DefaultIfEmpty(Result.Fail<IDrawable>("Empty tags"))
                .Aggregate(Result.Ok(), (prev, curr) => from _ in prev
                    from drawable in curr
                    from value in Draw(graphics, drawable, bounds)
                    select value);
        }

        private static Result<None> Draw(Graphics graphics, IDrawable drawable, Rectangle bounds)
        {
            if (!bounds.Contains(drawable.Bounds))
                return Result.Fail<None>("Image cannot contain all rectangles");
            drawable.Draw(graphics);
            return Result.Ok();
        }
    }
}