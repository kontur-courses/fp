using System.Collections.Generic;
using System.Drawing;
using TagsCloud.Visualization.ColorGenerators;

namespace TagsCloud.Visualization.LayoutContainer
{
    public interface ILayoutContainer<out T>
    {
        IReadOnlyList<T> Items { get; }
        Size Size { get; }
        Point Center { get; }

        void Draw(Graphics graphics, IColorGenerator colorGenerator);
    }
}