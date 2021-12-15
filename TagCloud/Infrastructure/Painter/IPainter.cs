using System.Drawing;
using TagCloud.Infrastructure.Monad;

namespace TagCloud.Infrastructure.Painter;

public interface IPainter
{
    Result<Bitmap> CreateImage(Dictionary<string, int> weightedWords);
}