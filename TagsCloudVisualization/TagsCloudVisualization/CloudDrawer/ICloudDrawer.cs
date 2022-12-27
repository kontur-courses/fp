using System.Drawing;
using ResultOf;

namespace TagsCloudVisualization.CloudDrawer;

public interface ICloudDrawer
{
    Result<bool> TryDraw(List<TextLabel> wordsInPoint);
}