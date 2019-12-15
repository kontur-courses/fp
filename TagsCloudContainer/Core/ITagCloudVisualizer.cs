using System.Drawing;
using ResultOf;

namespace TagsCloudContainer.Core
{
    public interface ITagCloudVisualizer
    {
        Result<Bitmap> GetTagCloudBitmap(Parameters parameters);
    }
}