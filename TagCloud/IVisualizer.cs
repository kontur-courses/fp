using System.Drawing;
using TagCloud.Templates;

namespace TagCloud;

public interface IVisualizer
{
    Result<Bitmap> Draw(ITemplate template);
}