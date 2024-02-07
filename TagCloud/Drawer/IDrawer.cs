using System.Drawing;
using ResultOf;

namespace TagCloud.Drawer;

public interface IDrawer
{
    Result<Bitmap> DrawTagCloud(IEnumerable<(string word, int rank)> words);
}