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
            var bounds = new Rectangle(Point.Empty, size);
            var helper = new DrawHelper(graphics, bounds);
            return Result.Ok()
                .ValidateNonNull(graphics, nameof(graphics))
                .Then(() => drawables
                    .Select(tag => tag.Then(t => t.Shift(Size.Truncate(size / 2f))))
                    .DefaultIfEmpty(Result.Fail<IDrawable>("Empty tags"))
                    .Aggregate(Result.Ok(), helper.ContinueDraw));
        }
    }
}