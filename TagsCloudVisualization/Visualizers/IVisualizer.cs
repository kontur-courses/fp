using Results;
using System.Drawing;
using TagsCloudVisualization.Tags;

namespace TagsCloudVisualization.Visualizers;

public interface IVisualizer
{
    Result<Bitmap> Vizualize(IEnumerable<Result<Tag>> tags);
}