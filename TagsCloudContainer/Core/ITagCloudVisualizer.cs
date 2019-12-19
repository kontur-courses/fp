using ResultOf;
using System.Drawing;

namespace TagsCloudContainer.Core
{
    public interface ITagCloudVisualizer
    {
        Result<Bitmap> GetTagCloudBitmap(Parameters parameters);
    }
}