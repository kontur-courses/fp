using System.Drawing;
using TagCloud.Templates;

namespace TagCloud;

public interface IVisualizer
{
    Bitmap Draw(ITemplate template);
}