using System.Drawing;
using ResultMonad;

namespace TagsCloudDrawer.Drawer
{
    public class DrawHelper
    {
        private readonly Graphics _graphics;
        private readonly Rectangle _bounds;

        public DrawHelper(Graphics graphics, Rectangle bounds)
        {
            _graphics = graphics;
            _bounds = bounds;
        }

        public Result<None> ContinueDraw(Result<None> previousResult, Result<IDrawable> drawableResult) =>
            from _ in previousResult
            from drawable in drawableResult
            from drawResult in Draw(drawable)
            select drawResult;

        private Result<None> Draw(IDrawable drawable) =>
            Result.Ok()
                .Validate(() => _bounds.Contains(drawable.Bounds), "Image cannot contain all tags")
                .Then(() => Result.Of(() => drawable.Draw(_graphics), "Cannot draw tag"));
    }
}