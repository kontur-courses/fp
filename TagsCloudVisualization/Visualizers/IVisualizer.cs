using Results;
using System.Drawing;
using TagsCloudVisualization.Tags;

namespace TagsCloudVisualization.Visualizers;

public interface IVisualizer
{
    Result<Bitmap> Vizualize(Result<IList<Tag>> tags);
}