using System.Collections.Generic;
using System.Drawing;
using App.Implementation.Words.Tags;
using App.Infrastructure.Visualization;

namespace App.Implementation.Visualization
{
    public class Visualizer : IVisualizer
    {
        private readonly IDrawer drawer;

        public Visualizer(IDrawer drawer)
        {
            this.drawer = drawer;
        }

        public Bitmap VisualizeCloud(Bitmap image, Point cloudCenter, IEnumerable<Tag> tags)
        {
            drawer.DrawTags(image, tags);
            return image;
        }

        public void VisualizeDebuggingMarkupOnImage(Image image, Point cloudCenter, int cloudCircleRadius)
        {
            drawer.DrawCanvasBoundary(image);
            drawer.DrawAxis(image, cloudCenter);
            drawer.DrawCloudBoundary(image, cloudCenter, cloudCircleRadius);
        }
    }
}