using System.Drawing;
using TagsCloud.Visualization.LayoutContainer;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.Drawers
{
    public interface IDrawer
    {
        Result<Image> Draw<T>(ILayoutContainer<T> layoutContainer);
    }
}