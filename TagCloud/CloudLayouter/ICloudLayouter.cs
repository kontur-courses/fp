using System.Collections.Generic;
using System.Drawing;
using ErrorHandling;
using TagCloud.Visualization;

namespace TagCloud.CloudLayouter
{
    public interface ICloudLayouter
    {
        ImageSettings LayouterSettings { get; }
        HashSet<Rectangle> Rectangles { get; set; }
        void ResetLayouter();
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}