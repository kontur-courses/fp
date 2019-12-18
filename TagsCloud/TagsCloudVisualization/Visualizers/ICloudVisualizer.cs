using System.Collections.Generic;
using System.Drawing;
using TagCloudResult;
using TagsCloudVisualization.Styling;
using TagsCloudVisualization.Styling.Themes;

namespace TagsCloudVisualization.Visualizers
{
    public interface ICloudVisualizer
    {
        Result<Bitmap> Visualize(ITheme theme, IEnumerable<RectangleF> rectangles,
            int width = 1000, int height = 1000);

        Result<Bitmap> Visualize(Style style, IEnumerable<Tag> tags,
            int width = 1000, int height = 1000);
    }
}