using ResultOf;
using TagCloud.Visualization.WordPainting;

namespace TagCloud.App
{
    public interface IWordPainterFabric
    {
        Result<IWordPainter> GetWordPainter();
    }
}