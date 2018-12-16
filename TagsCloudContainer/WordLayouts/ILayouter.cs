using System.Drawing;

namespace TagsCloudContainer.WordLayouts
{
    public interface ILayouter
    {
        RectangleF GetNextPosition(SizeF size);
        ILayouter WithConfig(ILayouterConfig size);
    }
}