using System.Collections.Generic;
using System.Drawing;

namespace TagsCloud.Visualization.LayoutContainer
{
    public interface ILayoutContainer<out T> : IVisitable
    {
        IReadOnlyList<T> Items { get; }
        Size Size { get; }
        Point Center { get; }
    }
}