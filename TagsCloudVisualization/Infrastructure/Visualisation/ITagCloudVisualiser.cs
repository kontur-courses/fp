using System.Drawing;

namespace TagsCloudVisualization.Infrastructure.Visualisation
{
    public interface ITagCloudVisualiser
    {
        Image Render(Tag[] tags, Size resolution, Color background);
    }
}