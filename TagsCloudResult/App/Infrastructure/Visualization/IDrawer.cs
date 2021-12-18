using System.Collections.Generic;
using System.Drawing;
using App.Implementation.Words.Tags;

namespace App.Infrastructure.Visualization
{
    public interface IDrawer
    {
        void DrawCanvasBoundary(Image image);

        void DrawAxis(Image image, Point cloudCenter);

        void DrawCloudBoundary(Image image, Point cloudCenter, int cloudCircleRadius);

        void DrawTags(Image image, IEnumerable<Tag> tags);
    }
}