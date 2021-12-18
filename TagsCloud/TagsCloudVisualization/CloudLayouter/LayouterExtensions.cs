using System.Drawing;
using ResultMonad;

namespace TagsCloudVisualization.CloudLayouter
{
    public static class LayouterExtensions
    {
        public static Result<Rectangle> PutNextRectangle(this ILayouter layouter, Size size)
        {
            return PositiveSize.Create(size.Width, size.Height)
                .Then(layouter.PutNextRectangle);
        }
    }
}