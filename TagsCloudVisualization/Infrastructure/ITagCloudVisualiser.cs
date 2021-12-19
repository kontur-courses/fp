using System.Drawing;

namespace TagsCloudVisualization.Infrastructure
{
    public interface ITagCloudVisualiser
    {
        Image Render(Tag[] tags, Size resolution, Color background);
    }
}