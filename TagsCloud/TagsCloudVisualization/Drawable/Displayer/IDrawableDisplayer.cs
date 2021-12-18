using System.Collections.Generic;
using ResultMonad;
using TagsCloudDrawer;

namespace TagsCloudVisualization.Drawable.Displayer
{
    public interface IDrawableDisplayer
    {
        Result<None> Display(IEnumerable<IDrawable> drawables);
    }
}