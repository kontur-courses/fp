using System.Drawing;
using TagsCloudContainer.Functional;
using TagsCloudContainer.Visualization.Measurers;

namespace TagsCloudContainer.Visualization.Painters
{
    public interface IPainter
    {
        Result<ColorizedRectangle[]> Colorize(Rectangle[] rectangles);

        Result<ColorizedTag[]> Colorize(Tag[] tags);
    }
}